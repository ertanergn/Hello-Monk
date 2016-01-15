Hello Monk Project
==================

<i>This is project is built as a sample project for MediaMonks.<i/>

The project contains following technologies
===========================================

- [ASP.Net MVC 4](http://www.asp.net/mvc/mvc4)
- [Ninject](http://www.ninject.org/)
- [NHibernate](http://nhibernate.info/)
- [Log4Net](https://logging.apache.org/log4net/)
- [Bootstrap](http://getbootstrap.com/)
- [JQuery](https://jquery.com/)
- [jqBootstrapValidation](http://reactiveraven.github.io/jqBootstrapValidation/)
- [CSS3](http://www.w3schools.com/css/css3_intro.asp)

Tools
=====

- [Visual Studio 2012](https://www.visualstudio.com/)
- [SQL Server 2014](https://www.microsoft.com/en-US/server-cloud/products/sql-server/default.aspx?WT.srch=1&WT.mc_ID=SEM_b7wrPDxD)
- [Google Chrome](https://www.google.com/chrome/browser/desktop/) for UI testing
- [NUnit](http://www.nunit.org/) for creating database and internal testing

How to run the project
======================

<i>The project can be hosted in IIS or debuged by Visual Studio. The application requires SQL Server connection and database. <i/>

1. Open SQL Server Management Studio and create a database named "Monk".
2. The applicattion uses integrated security to connect SQL Server, make sure IIS application pool has necessary access to SQL Server
3. Build solution
4. Run unit test named as "CreateEmptyDatabase" in assemble under "src\Test Monk\bin\{Build-Configuration}\Monk.Test.dll" with NUnit. This test create base database tables for the application
5. Finally open the application using a web browser.