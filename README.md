# API Docs:
## Identity API

> [!IMPORTANT]
**Base url**
`http://localhost:5000/api/identity`

### Login for students
**Method: POST**\
**Url:** `http://localhost:5000/api/identity/login`\
**Body (example json):**
```
{
  "Email": "email@gmail.com",
  "Password": "fvfvchcjkvjdkscjxbcnxc",
}
```
**Response:** authorization token (string)

### Login for employees
**Method: POST**\
**Url:** `http://localhost:5000/api/identity/employee_login`\
**Body (example json):**
```
{
  "Email": "email@gmail.com",
  "Password": "fvfvchcjkvjdkscjxbcnxc",
}
```
**Response:** authorization token (string)

### Registration
**Method: POST**\
**Url:** `http://localhost:5000/api/identity/reg`\
**Body (example json):**
```
{
  "Email": "email@gmail.com",
  "Password": "fvfvchcjkvjdkscjxbcnxc",
}
```
**Response:** Status Code
```
BadRequest (400) - Email already used
Ok (200) - User created successfully
```

## Organization API

> [!IMPORTANT]
**Base url**
`http://localhost:5006/api/organization`

### Get in range
**Method: Get**\
**Params: startId - from 0, count = from 1 to 20**
**Url:** `http://localhost:5006/api/organization/range?startId=0&&count=20`\
**Authorization:** Bearer
**Response: (C#)** 
```csharp
{
public class OrganizationModel : IEntity
{
	public int Id { get; set; }
	public required string FullName { get; set; }
	public required string ShortName { get; set; }
	public required List<GroupModel> Groups { get; set; }
}

public class GroupModel : IEntity
{
	public int Id { get; set; }
	public required string Name { get; set; }
}
```
