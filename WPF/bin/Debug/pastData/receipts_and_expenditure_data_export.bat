@echo off

REM #####################
REM ★接続情報
REM #####################
REM サーバー名(サーバーのPC名\インスタンス名)
set dbServer=192.168.44.115\SQLEXPRESS
REM データベース名
set dbName=accounting_process

REM tempファイルのパス
set tempFilePath=.\temp.csv

set YYYYMMDD=%DATE:/=%

REM CSVファイルのパス
set csvFilePath=.\past_receipts_and_expenditure_data%YYYYMMDD%.csv

REM 実行するSQL文(SELECT文) 出納データビューの去年末以前のデータを対象にする
set sql=SET NOCOUNT ON; SELECT * FROM reference_receipts_and_expenditure_data_view where registration_date between '1900-01-01' and cast(year(getdate())-1 as varchar)+'-12-31'

REM SELECT文を実行しtempファイルへ出力　※区切り文字をカンマ、余分なスペースを削除
sqlcmd -S %dbServer% -d %dbName% -E -s "," -W -o %tempFilePath% -Q "%sql%" -h -1

REM tempファイルの先頭が「-」でない行をCSVファイルへ出力　※2行目の「---」を取り除く
findstr /V "^-" %tempFilePath% > %csvFilePath%

REM tempファイルを削除
del %tempFilePath%

REM 今年から10年以前のcsvファイルを削除
forfiles /d -3650 /m "*.csv" /c "cmd /c del @file"

copy *.csv AllData\PastReferenceReceiptsAndExpenditureDataView.csv

pause