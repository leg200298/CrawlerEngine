CrawlerEngine
===
![downloads](https://img.shields.io/github/downloads/atom/atom/total.svg)
![build](https://img.shields.io/appveyor/ci/:user/:repo.svg)
![chat](https://img.shields.io/discord/:serverId.svg)

## Table of Contents

[TOC]

design
---
(https://drive.google.com/file/d/1VM8OUCgJG9Scf7f5sDjSgD2Uwp9_anaT/view?usp=sharing)

Job flows
---
```sequence
Note right of Engine: Resource and  Job
Engine->JobWorker: Get Info
Note right of Crawler: If Web-base Get resource
JobWorker-->Crawler: Craw Html Info!
Crawler-->Driver: Get Driver
Driver-->Crawler: Get free Driver
Crawler-->JobWorker: Get Html Data
JobWorker-->JobWorker: Validation
JobWorker-->JobWorker: Parse
JobWorker-->JobWorker: Save
JobWorker-->JobWorker: Has next  page
```

> Read more about sequence-diagrams here: http://bramp.github.io/js-sequence-diagrams/

## Appendix and FAQ

:::info
**Find this document incomplete?** Leave a comment!
:::

###### tags: `Templates` `Documentation`