- [Extension](#extension)
- [Manifest](#manifest)
- [Script](#script)
  - [Runtime](#runtime)
    - [localStorage](#localstorage)
    - [console](#console)
    - [`fetch(url: string [, init: FetchInit]): FetchResponse`](#fetchurl-string--init-fetchinit-fetchresponse)
      - [FetchInit](#fetchinit)
      - [FetchResponse](#fetchresponse)
      - [FetchHeader](#fetchheader)
    - [`parseHtml(rawHtml: string): HtmlElement`](#parsehtmlrawhtml-string-htmlelement)
      - [HtmlElement](#htmlelement)
    - [`unpack(script: string): string`](#unpackscript-string-string)
    - [`decodeLzStringFromBase64(base64: string): string`](#decodelzstringfrombase64base64-string-string)
  - [Functions](#functions)
    - [canModifyRequest](#canmodifyrequest)
    - [modifyRequest](#modifyrequest)
    - [requireLogin](#requirelogin)
    - [login](#login)
    - [home](#home)
    - [search](#search)
    - [detail](#detail)
    - [canReadChapter](#canreadchapter)
    - [loadChapterImages](#loadchapterimages)
    - [loadGalleryImagePages](#loadgalleryimagepages)
    - [loadImageFromPage](#loadimagefrompage)

# Extension

Extension allow you to write Javascript to extend the comic source

Like most of the Browser Extensions, iHentai extension require a `manifest.json` file and a Javascript file in the same folder.

# Manifest
A simple manifest file like this:
```
{
    "name": "ExHentai",
    "entry": "index.js",
    "version": 1,
    "hosts": [
        "exhentai.org",
        "forums.e-hentai.org",
        ".*\\.e-hentai\\.org",
        ".*\\.exhentai\\.org"
    ]
}
```
`name` is required as the extension name, and also being used as extension id (for now)  
`entry` is required as it define the relative path of the Javascript file  
`version` is the extension version, in int  
`hosts` define the domains your extension will send request to, you can think it as a allowlist, any request that not in the list will be rejected.  

<details><summary>All manifest properties</summary>

| Field            | Description                                                                   | Type     | Required |
| ---------------- | ----------------------------------------------------------------------------- | -------- | -------- |
| name             | required as the extension name, and also being used as extension id (for now) | string   | Yes      |
| entry            | required as it define the relative path of the Javascript file                | string   | Yes      |
| version          | extension version                                                             | int      | Yes      |
| hosts            | allowlist, any request that not in the list will be rejected                  | string[] | Yes      |
| icon             | icon path for the extension                                                   | string   | no       |
| manifest_version | manifest version                                                              | int      | no       |
| description      | extension description                                                         | string   | no       |
| author           | author for the extension                                                      | string   | no       |

</details>  

# Script
Script will only be the actual logic for fetching data from remote, UI code like HTML is not allowed.  
iHentai call the functions that defined in the extension's script, and script return cetern type of object. for example, to fetch comic list, iHentai will call `home` function with a `page` parameter, and the script return a list of gallery object.  
iHentai has its own runtime objects/functions/properties, it's slightly different from node/web.  

NOTE: 
- iHentai uses [Jint](https://github.com/sebastienros/jint) as Javascript engine for now, which is targeting **ECMAScript 5.1**.  
- Using webpack is not recommended.  
- Please do **NOT** use global varable for memory storage, since there will be mutiple instances of Javascript engine.  

## Runtime 

### localStorage  
Like the [`Window.localStorage`](https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage)
- `localStorage.setItem(key: string, value: string)`  
    When passed a key name and value, will add that key to the storage, or update that key's value if it already exists.
    | Name   | Description                                                                      | Type   |
    | ------ | -------------------------------------------------------------------------------- | ------ |
    | key    | A string containing the name of the key you want to create/update                | string |
    | value  | A string containing the value you want to give the key you are creating/updating | string |
    | return |                                                                                  | void   |
- `localStorage.getItem(key: string): string`  
    When passed a key name, will return that key's value.
    | Name   | Description                                                                             | Type    |
    | ------ | --------------------------------------------------------------------------------------- | ------- |
    | key    | A string containing the name of the key you want to retrieve the value of               | string  |
    | return | A string containing the value of the key. If the key does not exist, `null` is returned | string? |
### console  
Like the [`Window.console`](https://developer.mozilla.org/en-US/docs/Web/API/Window/console)
- `console.log(value: string) ` 
    Outputs a message to the debug console, usually being used while debugging.
    | Name   | Description                                    | Type   |
    | ------ | ---------------------------------------------- | ------ |
    | value  | A string that will output to the debug console | string |
    | return |                                                | void   |

### `fetch(url: string [, init: FetchInit]): FetchResponse`  
It's slightly different from [`fetch`](https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API), it return the actual `FetchResponse` not a promise, so it's not awaitable. It's a blocking call from C#.  
| Name   | Description                                                                   | Type          |
| ------ | ----------------------------------------------------------------------------- | ------------- |
| url    | A string containing the direct URL of the resource you want tofetch           | string        |
| init   | An object containing any custom settings that you want to apply to therequest | FetchInit?    |
| return | FetchResponse that represents the response to arequest.                       | FetchResponse |
<br/>

#### FetchInit

| Name     | Description                                                      | Type                 |
| -------- | ---------------------------------------------------------------- | -------------------- |
| method   | The request method, e.g., `GET`, `POST`, Default to `GET`        | string?              |
| referrer | A string specifying the referrer of the request.                 | string?              |
| headers  | A Map<string, string> headers you want to add to your request.   | Map<string, string>? |
| body     | A Map<string, string> body that you want to add to your request. | Map<string, string>? |
| bodyType | The body type, e.g., `UrlEncoded`                                | string?              |

<br/>

#### FetchResponse  
- `FetchResponse.text(): string`  
    Response body into string.

| Name       | Description                                                                                   | Type        |
| ---------- | --------------------------------------------------------------------------------------------- | ----------- |
| headers    | The `FetchHeader` object associated with theresponse.                                         | FetchHeader |
| ok         | A boolean indicating whether the response was successful (status in therange 200–299) or not. | bool        |
| status     | The status code of the response. (This will be 200 for a success.                             | int         |
| statusText | The status message corresponding to the status code. (e.g., OK for 200.                       | string      |
| url        | The URL of theresponse.                                                                       | string      |
| body       | Response body instring.                                                                       | string      |
| text()     | Response body intostring.                                                                     | string      |
<br/>

#### FetchHeader
- `FetchHeader.get(key: string): string`  
Returns a string sequence of all the values of a header within a Headers object with a given name.
- `FetchHeader.has(key: string): bool`  
Returns a boolean stating whether a Headers object contains a certain header.

### `parseHtml(rawHtml: string): HtmlElement`  
Parse HTML content into `HtmlElement`
<br/>

#### HtmlElement
- `HtmlElement.querySelector(selector: string): HtmlElement?`
- `HtmlElement.querySelectorAll(selector: string): List<HtmlElement>?`
- `HtmlElement.text(): string?`  
Return `textContent` from current html element
- `HtmlElement.html(): string?`  
Return `innerHtml` from current html element
- `HtmlElement.attr(name: string): string?`  
Return attribute value within given name, null if attribute not found
<br/>

### `unpack(script: string): string`  
Provide a pure Javascript environment for executing Javascript, return execute result in string

### `decodeLzStringFromBase64(base64: string): string`  
Decode base64 lzstring

## Functions

### canModifyRequest

### modifyRequest

### requireLogin

### login

### home

### search

### detail

### canReadChapter

### loadChapterImages

### loadGalleryImagePages

### loadImageFromPage

