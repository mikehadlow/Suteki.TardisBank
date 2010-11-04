Tardis Bank
-----------

Online pocket money banking for parents and kids.

http://tardisbank.com
http://github.com/mikehadlow/Suteki.TardisBank

Licence: Apache 2.0
http://www.apache.org/licenses/LICENSE-2.0.html

Based on:
Microsoft .NET 4.0
MVC3
Windsor
RavenDb
960Grid

Blog Posts:
http://mikehadlow.blogspot.com/2010/11/tardis-bank-is-now-live.html
http://mikehadlow.blogspot.com/2010/10/ravendb-indexes-1st-attempt.html
http://mikehadlow.blogspot.com/2010/10/tardis-bank-online-pocket-money-banking.html
http://mikehadlow.blogspot.com/2010/10/ravendb-aspnet-mvc-and-windsor-working.html
http://mikehadlow.blogspot.com/2010/10/ravendb-playing-with-inheritance-and.html

How to get it working
---------------------

1. Get the source code.
1a. In Web.Config change the line: <specifiedPickupDirectory pickupDirectoryLocation="c:\temp\maildrop\"/> to point to a location where the application has write access.
2. Download RavenDb and follow the instructions to start the RavenDb server on port 8080 (localhost).
3. Open the solution file in Visual Studio 2010
4. Hit F5, you should be able to browse the site.