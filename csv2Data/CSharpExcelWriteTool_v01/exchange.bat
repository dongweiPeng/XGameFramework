rem @echo off
set opMode=%1
set excel_path=%2
set jsonpath=%3
set col_strs=%4
set writefile=%5
set mapId=%6

if %opMode% GTR 0 (
::–¥»Îexcel
start /wait OfficeExcelHelper.exe %opMode% %jsonpath% %writefile%
) else (
::∂¡»°excel
start /wait OfficeExcelHelper.exe %opMode% %excel_path% %jsonpath% %mapId% %col_strs%
)

pause
