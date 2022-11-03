using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Services;

namespace Todo.Controllers
{
  public class GravatarController : ControllerBase
  {
    private static readonly string GravatarUrl = "https://en.gravatar.com/{0}.json";
    private HttpClient _httpClient;

    public GravatarController()
    {
      _httpClient = new HttpClient();
      // This is probably quite naughty - faking a browser user agent string.
      _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36");
    }

    [HttpGet]
    public async Task<IActionResult> Index(string email)
    {
      string gravatarData = "";
      string url = String.Format(GravatarUrl, Gravatar.GetHash(email));
      try
      {
        gravatarData = await _httpClient.GetStringAsync(url);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }

      // Ok() returns a 200 response with the content.
      return Ok(gravatarData);
    }
  }
}