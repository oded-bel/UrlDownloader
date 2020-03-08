# UrlDownloader

UrlDownloader is a .NET Framework solution for downloading Urls into a configurable location.


## Projects

#### UrlDownloader

The core classes and business logic implementing the downloading and file writing.

#### UrlDownloaderTests

A test project featuring sanity and unit tests.

#### UrlDownloaderConsoleApp

A simple console app that gets urls as a space separated string, and downloads the urls to a configurable location.

## Implementation Notes

Due to time constraints, the main logic is implemented through one consumer per type of url prefix, currently only supporting Http and Ftp.

Although both formats have a resume ability, only Ftp resume was implemented and Http is used as an example of a format that has no resume.

In an ideal solution, we would have the download managers run as seperate services taking input from persistant queues, with a Producer-Consumer service publishing to these queues from the url list.

In this solution we used DownloadManager as an abstract class, which has different implementations for the acquirement of file streams and resuming, based on the prefix it supports.

DownloadOrchastrator is the class we use as a Producer (although due to time constraints it is a rather simple closed producer).

The tests cover only very basic flaws for FTP and HTTP, which ofcourse in an ideal solution we would have far greater coverage, with a different solution for sanity/integration testing, so it could run as CI.

### Author
Oded Belahousky, 
odedbel@gmail.com