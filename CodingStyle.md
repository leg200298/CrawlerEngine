﻿# Coding Style
# C#
- 變數(必為名詞)Examples:商品明細(product detail)
  - Function內變數 
    - (camelCase)
    - Ex: productDetail 
  - Constant常數
    - 全大寫+_
    - Ex: PRODUCT_DETAIL 
  - Public變數
    - (PascalCase)
    - Ex: ProductDetail 
  - pravate變數
    - (camelCase)
    - Ex: productDetail 
  - 關鍵字變數
    - _+(camelCase)
    - Ex: _productDetail
  - 特殊狀況
    - i,x,y,e 使用場景function內計數參數
    - Ex: for(int i = 0 ;i<count;++i)

- Function(動詞+名)
  - (PascalCase)
  - Ex:GetEmailUser()

- Class(DB)
  - 遵照DB Style
  - 只能有data敘述跟attr
  - Ex
```
 public class CrawlDataJobListDto
    {
        [Column("seq")]
        public Guid seq { get; set; }
        [Column("job_type")]
        public string job_type { get; set; }
   
    }
```

- Class(名詞)
  - (PascalCase)
  - 須注意封裝完整性跟保護繼承屬性
  - 妥善利用 public ,protected,internal ,private
  
- Interface(I+名詞)
  - (PascalCase)
  - Ex: IJobWorker

- Csproj
  - X.XX 
    - X:主專案名稱
    - XX:輔助專案名稱
  - EX
    - Main
    - Main.DB
    - Main.Service
    
- Class繼承
  - Abstract 限縮流程
  - Class 父給予子能力
  - Interface 限定行為

# Javascript
- 使用空格代替tab
  ```
  // bad
  function foo() {
  ∙∙∙∙let name;
  }
  // bad
  function bar() {
  ∙let name;
  }
  // good
  function baz() {
  ∙∙let name; 
  }
  ```
    
- 不能省略分號
  ```
    // bad
    let luke = {}
    let leia = {}
    [luke, leia].forEach(jedi => jedi.father     = 'vader')
    // good
    let luke = {};
    let leia = {};
    [luke, leia].forEach((jedi) => {
    jedi.father = 'vader';
    });
  ```
    
- 暫時不要使用ES6 module
- 不推薦程式碼水平對齊
 
  ```
  // bad
  {
  tiny:  42, 
  longer: 435, 
  };
  // good
  {
      tiny: 42, 
      longer: 435,
  };
  ```
- 杜絕var
  ```
  // bad
  var example = 42;
  // good
  const example = 42;
  ```
- 優先使用箭頭函式
  ```
  // bad
  [1, 2, 3].map(function (x) {
  const y = x   1;
  return x * y;
  });
  // good
  [1, 2, 3].map((x) => {
  const y = x   1;
  return x * y;
  });
  ```
- 使用模板字串取代連線字串
  ```
  // bad
  function sayHi(name) {
  return 'How are you, '   name   '?';
  }
  // bad
  function sayHi(name) {
  return ['How are you, ', name, '?'].join();
  }
  // bad
  function sayHi(name) {
  return `How are you, ${ name }?`;
  }
  // good
  function sayHi(name) {
  return `How are you, ${name}?`;
  }
  ```
- 不要使用續行符分割長字串
  ```
  // bad (建議在PC端閱讀)
  const longString = 'This is a very long string that \
  far exceeds the 80 column limit. It unfortunately \
  contains long stretches of spaces due to how the \
  continued lines are indented.';
  // good
  const longString = 'This is a very long string that '   
  'far exceeds the 80 column limit. It does not contain '   
  'long stretches of spaces since the concatenated '  
  'strings are cleaner.';
  ```
- 優先使用for…of
  ```
  ```
- 不要使用eval語句
  ```
  // bad
  let obj = { a: 20, b: 30 };
  let propName = getPropName(); // returns "a" or "b"
  eval( 'var result = obj.'   propName );
  // good
  let obj = { a: 20, b: 30 };
  let propName = getPropName(); // returns "a" or "b"
  let result = obj[ propName ]; // obj[ "a" ] is the same as obj.a
  ```
- 常量的命名規範
  ```
  // bad
  const number = 5;
  // good
  const NUMBER = 5;
  ```
- 每次只宣告一個變數
  ```  
  // bad
  let a = 1, b = 2, c = 3;
  // good
  let a = 1;
  let b = 2;
  let c = 3;
  ```
- 使用單引號
  ```
  // bad
  let directive = "No identification of self or mission."
  // bad
  let saying = 'Say it ain\u0027t so.';
  // good
  let directive = 'No identification of self or mission.';
  // good
  let saying = `Say it ain't so`;
  ```

- ##### refs:<https://codertw.com/%E5%89%8D%E7%AB%AF%E9%96%8B%E7%99%BC/203839/>

# SQL

- 應該做的事情
  - 使用一致的、敘述性的名稱。
  - 靈活使用空格和縮排來增強可讀性。
  - 儲存符合 ISO-8601 標準的日期格式（YYYY-MM-DD HH:MM:SS.SSSSS）。
  - 最好使用標準 SQL 函式而不是特定供應商的函式以提高可移植性。
  - 保證程式簡潔明了並消除多餘的 SQL（比如非必要的引號或括號，或者可以推導出的多餘 WHERE 語句）。
  - 必要時在 SQL 程式碼中加入註解。優先使用 C 語言式的以 /* 開始以 */ 結束的區塊註解，或使用以 -- 開始的單行註解。
   ```
   SELECT file_hash  -- stored ssdeep hash
  FROM file_system
    WHERE file_name = '.vimrc';
   /* Updating the file record after writing to the file */
  UPDATE file_system
   SET file_modified_date = '1980-02-22 13:19:01.00000',
       file_size = 209732
    WHERE file_name = '.vimrc';
   ```
  - 應避免的事情
  - 駝峰命名法——它不適合快速掃描。
描述性的前綴或匈牙利命名法比如 sp_ 或 tbl。
  - 複數形式——盡量使用更自然的集合術語。比如，用“staff”替代“employees”，或用“people”替代“individuals”。
  - 需要引用號的標識符——如果你必須使用這樣的標識符，最好堅持用SQL92的雙引號來提高可移植性。
  - 物件導向程式設計的原則不該應用到結構化查詢語言或資料庫結構上。

- 一般原則
  - 保證名字獨一無二且不是保留字。
  - 保證名字長度不超過 30 個字節。
  - 名字要以字母開頭，不能以下底線結尾。
  - 只在名字中使用字母、數字和下底線。
  - 不要再名字中出現連續下底線——這樣很難辨認。
  - 在名字中需要空格的地方用下底線代替。
  - 盡量避免使用縮寫詞。使用時一定確定這個縮寫簡明易懂。
   ```
   SELECT first_name
  FROM staff;
  ```
- 表格（Tables）
  - 用集合名詞，或在不那麽理想的情況下，複數形式。如 staff 和 employees。
  - 不要使用類似 tbl 或其他的描述性的前綴或匈牙利命名法。
表不應該同它的列同名，反之亦然。
  - 盡量避免連接兩個表的名字作為關系表（relationship table）的名字。與其使用 cars_mechanics 做表名不如使用 services。
- 欄位名（Columns）
  - 總是使用單數形式。
  - 避免直接使用 id 做表的主標識符。
  - 避免列名同表名同名，反之亦然。
  - 總是使用小寫字母，除非是特殊情況，如專有名詞。
  - 別名與關聯名（Aliasing or correlations）
  - 應該與它們別名的對象或與它們代表的表達式相關聯。
  - 一般來說，關聯名應該是對象名的第一個字母。
  - 如果已經有相同的關聯名了，那麽在關聯名後加一個數字。
  - 總是加上 AS 關鍵字，因為這樣的顯示聲明易於閱讀。
  - 為計算出的數據命名時，用一個將這條數據存在表裏時會使用的列名。
   ```
   SELECT first_name AS fn
  FROM staff AS s1
  JOIN students AS s2
    ON s2.mentor_id = s1.staff_num;
   SELECT SUM(s.monitor_tally) AS monitor_total
  FROM staff AS s;
  ```
- 預存程序（Stored procedures）
  - 名字一定要包含動詞。
  - 不要附加 sp_ 或任何其他這樣的敘述性前綴或使用匈牙利表示法。
- 統一的後綴（Uniform suffix）
  - 下列後綴有統一的意義，能保證 SQL 程式碼更容易被理解。在合適的時候使用正確的後綴。

    - _id 獨一無二的標識符，如主鍵。
    - _status 標識值或任何表示狀態的值，比如publication_status。
    - _total 總和或某些值的和。
    - _num 表示該域包含數值。
    - _name 表示名字。
    - _seq 包含一系列數值。
    - _date 表示該列包含日期。
    - _tally 計數值。
    - _size 大小，如文件大小或服裝大小。
    - _addr 地址，有形的或無形的，如 ip_addr

---查詢語句（Query syntax）
- 保留字（Reserved words）
  - 保留字總是大寫，如 SELECT 和 WHERE。

  - 最好使用保留字的全稱而不是簡寫，用 ABSOLUTE 而不用 ABS。

  - 當標準 ANSI SQL 關鍵字能完成相同的事情時，不要使用資料庫伺服器相依的關鍵字，這樣能增強可移植性。
   ```
   SELECT model_num
  FROM phones AS p
    WHERE p.release_date > '2014-09-30';
    ```
- 空白字元（White space）
  - 正確地使用空白字元對清晰的程式碼十分重要。不要把程式碼堆在一起或移除自然語言中的空格。

  - 空格（Spaces）
  - 用空格使根關鍵字都結束在同一列上。在程式碼中形成一個從上到下的「川流」，這樣幫助讀者快速掃描程式碼並將關鍵字和實做細節分開。川流在排版時應該避免，但是對書寫 SQL 語句是有幫助的。
   ```
  (SELECT f.species_name,
        AVG(f.height) AS average_height, AVG(f.diameter) AS average_diameter
   FROM flora AS f
  WHERE f.species_name = 'Banksia'
     OR f.species_name = 'Sheoak'
     OR f.species_name = 'Wattle'
  GROUP BY f.species_name, f.observation_date)

  UNION ALL

   (SELECT b.species_name,
        AVG(b.height) AS average_height, AVG(b.diameter) AS average_diameter
   FROM botanic_garden_flora AS b
  WHERE b.species_name = 'Banksia'
     OR b.species_name = 'Sheoak'
     OR b.species_name = 'Wattle'
  GROUP BY b.species_name, b.observation_date);
  ```
  - 注意 WHERE 和 FROM 等關鍵字，都靠右對齊，而真實的列名都靠左對齊。

  - 下列是需要加入空格的情況：

  - 在等號前後（=）
  - 單引號前後（'）
  - 除非單引號後面是括號、逗號或分號
  ```
  SELECT a.title, a.release_date, a.recording_date
  FROM albums AS a
  WHERE a.title = 'Charcoal Lane'  OR
  a.title = 'The New Danger';
    ```
---換行（Line spacing）
- 必需換行的時機：

  - 在AND或OR前。
  - 在分號後（分隔語句以提高可讀性）。
  - 在每個關鍵字定以後。
  - 將多個列組成一個邏輯組時的逗號後。
  - 將程式碼分隔成相關聯的多個部分，幫助提高大區塊程式碼的可讀性。
  - 將所有的關鍵字右對齊，將所有的值靠左對齊，並在查詢語句中間留出一個空隙。這樣能提高程式碼的閱讀速度。
  ```
  INSERT INTO albums (title, release_date, recording_date)
  VALUES ('Charcoal Lane', '1990-01-01 01:01:01.00000', '1990-01-01 01:01:01.00000'),
  ('The New Danger', '2008-01-01 01:01:01.00000', '1990-01-01 01:01:01.00000');
  
  UPDATE albums
  SET release_date = '1990-01-01 01:01:01.00000' 
  WHERE title = 'The New Danger';
  SELECT a.title,a.release_date, a.recording_date, a.production_date -- grouped dates together
  FROM albums AS a
  WHERE a.title = 'Charcoal Lane' 
     OR a.title = 'The New Danger';
     ```
- 縮排（Indentation）
  - 為確保 SQL 的可讀性，務必要遵守下列規則。

- Join 語句
  - Join 語句應該縮進到川流的另一側並在必要的時候加入換行。
  ```
  SELECT r.last_name
  FROM riders AS r
       INNER JOIN bikes AS b
       ON r.bike_vin_num = b.vin_num
          AND b.engine_tally > 2

       INNER JOIN crew AS c
       ON r.crew_chief_last_name = c.last_name
          AND c.chief = 'Y';
    ```
  - 子查詢（Subqueries）
    - 子查詢應該在川流的右側對齊並使用其他查詢相同的樣式。有時候將右括號單獨置於一行並同與它配對的左括號對齊是有意義的——尤其是當存在嵌套子查詢的時候。
    ```
    SELECT r.last_name,
       (SELECT MAX(YEAR(championship_date))
          FROM champions AS c
         WHERE c.last_name = r.last_name
           AND c.confirmed = 'Y') AS last_championship_year
           FROM riders AS r
           WHERE r.last_name IN
       (SELECT c.last_name
          FROM champions AS c
         WHERE YEAR(championship_date) > '2008'
           AND c.confirmed = 'Y');
           ```
- 推薦的形式（Preferred formalisms）
  - 盡量使用 BETWEEN 而不是多個 AND 語句。
  - 同樣地，使用 IN() 而不是多個 OR 語句。
  - 當數據輸出資料庫時需要處理時，使用 CASE 表達式。CASE 語句能嵌套形成更覆雜的邏輯結構。
  - 盡量避免 UNION 語句和臨時表。如果資料庫架構能夠不依賴這些語句運行，那麽多數情況下它就不應該依賴這些語句。
  ```
  SELECT CASE postcode
       WHEN 'BN1' THEN 'Brighton'
       WHEN 'EH1' THEN 'Edinburgh'
       END AS city
  FROM office_locations WHERE country = 'United Kingdom'
   AND opening_time BETWEEN 8 AND 9
   AND postcode IN ('EH1', 'BN1', 'NN1', 'KW1');
   ```
   
---建立語句（Create syntax）
    聲明模式信息時維護可讀程式碼也很重要。所以列定義的順序和分組一定要有意義。

在 CREATE 語句中，其後每列要縮排 4 個空格。

- 選擇數據類型（Choosing data types）
  - 盡可能避免使用特定廠商的資料類型。 它們不僅不可移植，而且可能不適用於相同廠商的舊版軟體。
  - 只在真的需要浮點數運算的時候才使用 REAL 和 FLOAT 類型，否則使用 NUMERIC 和 DECIMAL 類型。浮點數進位誤差是個問題。
- 指定預設類型（Specifying default values）
  - 預設值一定與列的類型相同——如果一個列的類型是 DECIMAL 那麽就不要使用 INTEGER 類型作為預設值。
  - 預設值要緊跟類型聲明並在 NOT NULL 聲明前。
- 約束和鍵（Constraints and keys）
  - 約束（Constraints）和鍵（key）是構成資料庫系統的重要組成部分。它們能很快地變得難以閱讀和理解，所以遵從指南方針是很重要的。

--- 選擇鍵（Choosing keys）
  - 設計時應該謹慎選擇構成鍵的列，因為鍵既明顯影響著性能和數據完整性。

  1. 鍵在某種程度上應該是獨一無二的。
  2. 該值在不同表中的類型應該相同並且盡量不會更改。
  3. 該值是否會無法通過某些標準格式（如 ISO 發布的標準）？ 遵守第2點。
  4. 盡量讓鍵保持簡單，但在適當情況下不要害怕使用複合鍵。
  - 以上是定義資料庫時合乎邏輯的平衡做法。當需求變更時，鍵也應該根據情況更新。

--- 定義約束（Defining constraints）
確定鍵後，就可以用約束和字值段驗證來定義它們。

--- 概要（General）
  - 表至少需要一個鍵來保證其完整性和可用性。
  - 約束應該有名字，除了 UNIQUE、PRIMARY KEY 和 FOREIGN KEY 之外。
--- 布局和順序（Layout and order）
  - 在 CREATE TABLE 語句後先定義主鍵。
  - 約束的定義應該緊跟它相應的列的定義後。
  - 如果該約束與多個列相關，那麽讓它盡量離與其相關的列距離越近越好。實在不行就講它放在表定義的最後。
  - 如果是與整個表相關聯表級別的約束，那麽就將放在表的定義的最後。
  - 按照字母順序安排定義，ON DELETE 排在 ON UPDATE 前。
  - 必要的話，把所有相關的語句對齊。比如，把所有 NOT NULL 定義對齊到同一列。雖然這樣的做法有些慢，但是能提高可讀性。
--- 校驗（Layout and order）
  - 用 LIKE 和 SIMILAR TO 約束來保證格式已知字串的數據完整性。
  - 當數字的值的範圍可以確定時，用 CHECK() 來防止錯誤的值進入資料庫或被錯誤地轉換。在大部分情況下，至少要確認該值大於零。
  - CHECK() 約束應該在單獨的語句中以便偵錯。
--- 範例
```
CREATE TABLE staff (
    PRIMARY KEY (staff_num),
    staff_num      INT(5)       NOT NULL,
    first_name     VARCHAR(100) NOT NULL,
    pens_in_drawer INT(2)       NOT NULL,
                   CONSTRAINT pens_in_drawer_range
                   CHECK(pens_in_drawer >= 1 AND pens_in_drawer < 100)
);
```
- Designs to avoid
  - 物件導向程式設計並不適用於關聯式資料庫——避免這個陷阱。
  - 將值保存在一個欄位並將單位保存在另一個欄位。欄位的定義應該讓自己的單位不言自明以避免在應用程式內進行合並。使用CHECK()來保證資料庫中的數據是有效的。
  - EAV (Entity Attribute Value)表——用特殊的產品來處理無模式數據。
  - 因為某些原因（如為了歸檔、為了劃分跨國公司的區域）將能合並在一起的表分開。這樣的設計導致以後必須使用 UNION 操作而不能直接查詢一個表。

---附錄 資料庫關鍵字
- ##### refs:<https://www.sqlstyle.guide/zh-tw/#%E8%A1%A8%E6%A0%BCtables>