# MiDraplus project

React TS & Microservices API backend with SignalR

## 1. Installation

> Every installation part will start from root folder.

### 1.1 Client app - React TS

_If there is an error about not found **NPM** you can read NPM install guide at the Q&A part_

```
cd client
npm install
```

### 1.2 Server api - Microservices APIs with ASP.NET

_If there is an error about not found **Dotnet** you can read Dotnet install guide at the Q&A part_

Open terminal in approriate folder (api/name_service) and run:

```
dotnet restore
```

Work the same for other services

## 2. Run app in development

> Every run part will start from root folder.

### 2.1 Client app - React TS

```
cd client
npm serve
```

### 2.2 Server api - Microservices APIs with ASP.NET

Open terminal in approriate folder (api/name_service/NameService) and run:

```
dotnet watch run
```

Work the same for other services

## Q & A

-   Install NodeJS (ver 16.\*) with NPM included: [this link](https://nodejs.org/en/download/).

-   Install Dotnet (ver 6.0): [this link](https://dotnet.microsoft.com/en-us/download)

-   Install Docker: [this link](https://docs.docker.com/get-docker/)
