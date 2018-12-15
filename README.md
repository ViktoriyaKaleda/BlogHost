# BlogHost

Blog Host app allows you to:

- create blogs, choose blog styles, add moderators
- add posts to your blogs
- comment and like posts
- view most popular posts
- searching by titles, text or tags
- manage user accounts (admin panel)

<img src="Screenshot.png" width=70% height=70% />

#### Realization details

BlogHost project is written on ASP.NET Core 2.1.

3-layer architecture with stairway pattern allows both to evolve and vary project parts independently by organizing abstractions and implementations in separate assemblies.

Entity Framework Core (code first approach) is used in data access layer.

Login functionality implements by ASP.NET Core Identity, policy-based authorization is used for access restriction to app functionality.

English, Russian and Deutsch localizations are available in application.

#### TO DO:

- Extend admin panel
- Improve user interface
- Publish to Azure
