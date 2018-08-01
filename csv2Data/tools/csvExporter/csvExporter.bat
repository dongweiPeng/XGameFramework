set excelDir=%1
set csvDir=%2

start /wait Excel2CsvExporter.exe %excelDir% %csvDir%

pause