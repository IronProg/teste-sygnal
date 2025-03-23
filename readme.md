# teste-sygnal
This project was done following [Sygnal Group Test Apply Repo](https://github.com/sygnalgroup/Test-Apply) instructions.

## How to access?
This repository has 3 projects, and they are, in development order:
1. **Razor Pages** - ASP.NET Core Razor Pages with jQuery, deployed on Fly.io
2. **RestAPI** - ASP.NET Core API deployed in Fly.io
3. **ReactJS** - ReactJS app deployed in Vercel

### Razor Pages
This was the first one that I've made, because Razor is the technology I'm most used to. You can check out my Razor Pages version of the project in [this Fly.io link](https://testesygnal.fly.dev/api/rest).

> Technologies for App: Razor Pages, Entity Framework Core, jQuery 

> Technologies for Testing: XUnit, Microsoft.AspNetCore.Mvc.Testing

### RestAPI
This was the second one, implemented into the same host, with the same app (so i could deploy without any more costs). You can check out my the API consuming the url https://testesygnal.fly.dev/api/rest, has the following routes:

- GET /Order - Returns specific order
- - **Query Params**
- - ControlNumber:int - Filters result by a minimum control number
- - ControlNumberMax:int - Filters result by a maximum control number
- - State:string - Filters result by an specific state
```js
[
    {
        controlNumber: number,
        state: string // "pending", "inprogress", "completed"
    }
]
```
- GET /Order/{controlNumber:int} - Returns specific order
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
- DELETE /Order/{controlNumber:int} - Delete an pending order
```js
{
    controlNumber: number,
    state: string // "pending", "inprogress", "completed"
}
```

> Technologies for App: Razor Pages, Entity Framework Core, jQuery 

> Technologies for Testing: XUnit, Microsoft.AspNetCore.Mvc.Testing

### ReactJS
This one was generated with *create-react-app*. You can check out my ReactJS version of the project in [this Vercel link](https://teste-sygnal.vercel.app/).

> Technologies for App: ReactJS, TailwindCSS, Flowbite, React Toastify

> Technologies for Testing: jest, react-testing-library