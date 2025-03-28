﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public abstract class BaseHttpClient(HttpClient httpClient)
{
    protected async Task<T> GetAsync<T>(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var httpResponseMessage = await httpClient.SendAsync(request)
            .Result
            .EnsureSuccessStatusCode().Content.ReadAsStringAsync();

        if (httpResponseMessage == null) throw new HttpRequestException($"Unable to get response from url: {url}");
        var responseBody = JsonConvert.SerializeObject(httpResponseMessage);
        return JsonConvert.DeserializeObject<T>(responseBody);
    }

    protected async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddHeaders(headers, request);
        var httpResponseMessage = await httpClient
            .SendAsync(request)
            .Result
            .EnsureSuccessStatusCode()
            .Content
            .ReadAsStringAsync();

        if (httpResponseMessage == null) throw new HttpRequestException($"Unable to get response from url: {url}");
        var responseBody = JsonConvert.SerializeObject(httpResponseMessage);
        return JsonConvert.DeserializeObject<T>(responseBody);
    }

    protected async Task<T> GetAsync<T>(string url, Dictionary<string, string> headers,
        Dictionary<string, string> queryParameters)
    {
        url = AddQueryParameters(url, queryParameters);

        var request = new HttpRequestMessage(HttpMethod.Get, url);

        var httpResponseMessage = await httpClient
            .SendAsync(request)
            .Result
            .EnsureSuccessStatusCode()
            .Content
            .ReadAsStringAsync();

        if (httpResponseMessage == null) throw new HttpRequestException($"Unable to get response from url: {url}");

        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };

        return JsonConvert.DeserializeObject<T>(httpResponseMessage, settings);
    }

    private static string AddQueryParameters(string url, Dictionary<string, string> queryParameters)
    {
        var uriBuilder = new UriBuilder(url);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        uriBuilder.Query = query.ToString();
        foreach (var queryParameter in queryParameters) query[queryParameter.Key] = queryParameter.Value;
        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }

    private static void AddHeaders(Dictionary<string, string> headers, HttpRequestMessage request)
    {
        foreach (var header in headers) request.Headers.Add(header.Key, header.Value);
    }
}