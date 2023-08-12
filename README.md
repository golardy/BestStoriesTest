# BestStoriesTest
Dockerized aspnet core web api project that works with Hacker API

## Links
* [Hacker API](https://github.com/HackerNews/API)

## Notes
According to requiremnts the API should return data as it is in Hacker API and it should be:
```
[
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01+00:00",
    "score": 1716,
    "commentCount": 572
    },
    { ... },
    { ... },
    { ... },
    ...
]
```

According to HackerAPI documentations we have (looks like the API documentation was updated):

Field | Description
------|------------
by | The username of the item's author.
time | Creation date of the item, in [Unix Time](http://en.wikipedia.org/wiki/Unix_time).
url | The URL of the story.
score | The story's score, or the votes for a pollopt.
title | The title of the story, poll or job. HTML.
descendants | In the case of stories or polls, the total comment count.

As result the solutions retruned HackerAPI as it is and it will look:

```
[
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 1716,
    "descendants": 572
    },
    { ... },
    { ... },
    { ... },
    ...
]
```

The next assumption was made that data on Hacker API is not to often changed and it allows to use caching mechanism to exclude Hacker API overlaping.
Current approach allow handle a large amount of request witout huge amount request to Hacker API
It is also should be taken into consideration that first API calls can be longer than the next one because of cache is filled with data.

Logic for retriving data is to receive all data from Hacker API and then manipulate with this data. For example - the list of best stories contains 
5 elements:
```
[
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 30,
    "descendants": 1
    },
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 10,
    "descendants": 3
    },
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 40,
    "descendants": 572
    },
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 11,
    "descendants": 10
    },
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 60,
    "descendants": 30
    },
]
```

According to requirements the API should return an array of the first n "best stories" as returned by the Hacker News API, sorted by their score in a descending order. 
For our example if we request 3 items will be:
```
[
{
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 60,
    "descendants": 30
    },
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 40,
    "descendants": 572
    },
    {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "url": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "by": "ismaildonmez",
    "time": 123456789,
    "score": 30,
    "descendants": 1
    }
]
```

## Technologies
1. .Net 7.0
1. ASP.NET
1. docker

### Prerequisites
- Visual Studio 17 or higher/VS Code
- .NET 7.0 SDK (the project targets .NET 7.0), with a time a new version of sdk potentially can be used
- git

### Steps to deploy solution locally
1. You need to install the docker desktop client based on your machine (PC) as per your operating system type. Use the following link to download the docker desktop client. https://www.docker.com/products/docker-desktop/
2. In order to run app we need to build app image. Please execute command in Terminal (Windows) from root folder:
`docker build -t beststoryapi-image -f BestStories.Api\Dockerfile .`
3. Lets run container and execute the next command:
`docker run -d -p 8080:80 --name beststoryapicontainer beststoryapi-image`
4. Currently our application is available by address - http://localhost:8080/ and using Postman/Fillder the api can be send to address http://localhost:8080/beststories/2 and API return result:

```
[
    {
        "title": "Downfall Attacks",
        "url": "https://downfall.page/",
        "by": "WalterSobchak",
        "time": 1691515658,
        "score": 1310,
        "descendants": 327
    },
    {
        "title": "I'm 17 and wrote this guide on how CPUs run programs",
        "url": "https://github.com/hackclub/putting-the-you-in-cpu",
        "by": "archmaster",
        "time": 1691588039,
        "score": 1298,
        "descendants": 300
    }
]
```

Swagger API documentation will be available http://localhost:8080/swagger/index.html. It can be used for testing API

### Request structure 
API include the only one API endpoint `http://localhost:8080/beststories/{count}`.
-`{url} - addres locally deployed container with application`
-`{count} - amount of first best stories that will be taken and then sorted`
