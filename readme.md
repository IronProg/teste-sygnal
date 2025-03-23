# teste-sygnal
This project was done following [Sygnal Group Test Apply Repo](https://github.com/sygnalgroup/Test-Apply) instructions.

## How to access?
This repository contains three projects, developed in the following order:
1. **Razor Pages** - ASP.NET Core Razor Pages with jQuery, deployed on Fly.io
2. **RestAPI** - ASP.NET Core API deployed on Fly.io
3. **ReactJS** - ReactJS app deployed on Vercel

### [Razor Pages](https://testesygnal.fly.dev/)
This was the first project I created, as Razor is the technology I'm most familiar with.

> Technologies for the App: [Razor Pages](https://github.com/dotnet/razor) | [Entity Framework Core](https://github.com/dotnet/efcore) | [jQuery](https://jquery.com/) 

> Technologies for Testing: [XUnit](https://xunit.net/) | [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing)

### RestAPI
This was the second project. It was implemented within the same host and app to allow deployment without additional costs. You can interact with the API at:
> Base URL: https://testesygnal.fly.dev/api/rest

Here are the available routes:

- GET /Order - Returns a list of orders
- - **Query Params**
- - ControlNumber:int - Filters results by a **minimum** control number
- - ControlNumberMax:int - Filters results by a **maximum** control number
- - State:string - Filters result by an specific **state**
```js
[
    {
        controlNumber: number,
        state: string // "pending", "inprogress", "completed"
    }
]
```
- GET /Order/{controlNumber:int} - Returns a specific order
```js
{
    controlNumber: number,
    state: string // "pending", "inprogress", "completed"
}
```
- POST /Order - Creates new order
```js
{
    controlNumber: number,
    state: "pending"
}
```
- PUT /Order/{controlNumber:int} - Updates order state
```js
{
    controlNumber: number,
    state: string // "pending", "inprogress", "completed"
}
```
- DELETE /Order/{controlNumber:int} - Deletes a pending order
```js
{
    controlNumber: number,
    state: string // "pending", "inprogress", "completed"
}
```

> Technologies for the App: [Razor Pages](https://github.com/dotnet/razor) | [Entity Framework Core](https://github.com/dotnet/efcore) | [jQuery](https://jquery.com/) 

> Technologies for Testing: [XUnit](https://xunit.net/) | [Microsoft.AspNetCore.Mvc.Testing](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.Testing)

### [ReactJS](https://teste-sygnal.vercel.app/)
This one was generated with **create-react-app**.

> Technologies for the App: [ReactJS](https://react.dev/) | [TailwindCSS](https://tailwindcss.com/) | [Flowbite](https://flowbite.com/) | [React Toastify](https://fkhadra.github.io/react-toastify/introduction/)

> Technologies for Testing: [jest](https://jestjs.io/), [react-testing-library](https://testing-library.com)