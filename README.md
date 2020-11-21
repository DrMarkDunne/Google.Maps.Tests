## [Google.Maps.Tests](https://www.google.com/maps)
---

#### Tasks

Engineering challenage to use .Net Core C# with NUnit to test the Google Maps Web UI:
1. Go to [Google Maps](https://www.google.com/maps)
2. Enter Dublin in the search box
3. Search
4. Verify left panel has "Dublin" as a headline text
5. Click Directions icon
6. Verify destination field is "Dublin"

---

#### Notes

1. Use element locator methods as necessary**
2. Implement as many assertions as you deem applicable**

---

#### .Net Core 3.1

[Download .Net Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

Run tests using commandline tools:
```
dotnet clean
dotnet build
dotnet test
```

---

#### Power BI

View live results on the [Power BI Report](https://app.powerbi.com/view?r=eyJrIjoiZjI5ZTkwZjQtMDIyNy00MTYwLTkzZTEtYzJjOWI5OWQxZjNkIiwidCI6Ijg2ZWI1Y2RjLTA1ZDUtNDk1Mi1iMzZkLWJjMTEwYWYxZTJlNSIsImMiOjh9)

---

#### Allure Reports

[Download Commandline Allure](https://github.com/allure-framework/allure2)
![GitHub Logo](/allure_report.png)

Generate an Allure report using commandline tools:
```
c:\allure-commandline-2.13.6\bin\allure.bat serve C:\git\Google.Maps.Tests\Google.Maps.Tests\bin\Debug\netcoreapp3.1\allure-results
```

---

#### Run It All Together

PowerShell:
```
cd C:\git\Google.Maps.Tests\Google.Maps.Tests\ | dotnet clean | dotnet build --force --nologo | dotnet test --filter Category="CityTests" --nologo -- NUnit.DefaultTimeout=150000 | c:\allure-commandline-2.13.6\bin\allure.bat serve C:\git\Google.Maps.Tests\Google.Maps.Tests\bin\Debug\netcoreapp3.1\allure-results
```

---
