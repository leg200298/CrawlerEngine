#設計一個爬蟲引擎框架

design:(https://drive.google.com/file/d/1VM8OUCgJG9Scf7f5sDjSgD2Uwp9_anaT/view?usp=sharing) 

* CrawlerEngine<br>
   *核心啟動程式(決定資源)
* CrawlerEngine.Core<br>
   *啟動瀏覽器並且爬蟲取得資料
* CrawlerEngine.Driver<br>
   *掌管瀏覽器資源
* CrawlerEngine.JobWorker<br>
   *執行job的lifecycle
* CrawlerEngine.Models<br>
   *資源交互的models
* CrawlerEngine.Test<br>