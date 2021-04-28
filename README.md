# Curl HttpClient

+ This library will be an alternative to System.Net.Http.HttpClient.
+ Originaly started because of some things curl would do that newer dotnet would not.

## Examples

### GET
+ Basic GET Request looks like this
```c#
    var http = new nac.CurlHttpClient.http();
    var result = http.get("http://httpbin.org/ip");
```
+ It can also look like this
```c#
    var http = new nac.CurlHttpClient.http("http://httpbin.org/ip");
    var result = http.get();
```

### POST
+ Basic Post FormPost method
```c#
    var http = new nac.CurlHttpClient.http();
    var result = http.post("http://httpbin.org/post",
        requestBody: "fieldname1=fieldvalue1&fieldname2=fieldvalue2");
```
