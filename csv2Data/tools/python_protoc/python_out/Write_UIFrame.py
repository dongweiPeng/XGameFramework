# -*- coding: utf-8 -*-
import UIFrame_pb2
import codecs

import CommonMoudle


ParsePropertyName = ''
ParseTargetType = ''
ParsePropertyValue = ''
def addItem(item, args):
	global ParsePropertyName
	global ParseTargetType
	global ParsePropertyValue
	ParsePropertyName = 'ID'
	ParsePropertyValue = args[0]
	ParseTargetType = 'int' 
	item.ID = CommonMoudle.setProtoValuePyExcel(int, args[0], False)
	ParsePropertyName = 'WindowID'
	ParsePropertyValue = args[1]
	ParseTargetType = 'str' 
	item.WindowID = CommonMoudle.setProtoValuePyExcel(str, args[1], False)
	ParsePropertyName = 'Name'
	ParsePropertyValue = args[2]
	ParseTargetType = 'str' 
	item.Name = CommonMoudle.setProtoValuePyExcel(str, args[2], False)
	ParsePropertyName = 'FrameType'
	ParsePropertyValue = args[3]
	ParseTargetType = 'int' 
	item.FrameType = CommonMoudle.setProtoValuePyExcel(int, args[3], False)
	ParsePropertyName = 'BarContent'
	ParsePropertyValue = args[4]
	ParseTargetType = 'str' 
	item.BarContent = CommonMoudle.setProtoValuePyExcel(str, args[4], False)
	ParsePropertyName = 'JumpWndID'
	ParsePropertyValue = args[5]
	ParseTargetType = 'str' 
	item.JumpWndID = CommonMoudle.setProtoValuePyExcel(str, args[5], False)
	ParsePropertyName = 'JumpWndType'
	ParsePropertyValue = args[6]
	ParseTargetType = 'str' 
	item.JumpWndType = CommonMoudle.setProtoValuePyExcel(str, args[6], False)
	

table = UIFrame_pb2.UIFrameTable()
table.tname = 'UIFrame.bytes'
rf = CommonMoudle.open_excel('txt\\���ڱ�.xlsx')
sheet = rf.sheet_by_name('UIFrame')
rows = sheet.nrows
start_row = 3


for i in range(start_row, rows):
    line = sheet.row_values(i)
    if (len(line) == 0):break
    if (line[0] == ''):break
    try:
        FirstColVal = line[0]
        addItem(table.tlist.add(), line)
    except Exception,e:
        print e
        print 'addItem Erorr!!! at line %d , table: %s' % (i, table.tname)
        fmt_str = u'第一列值:%s,字段名:%s, 字段类型:%s, 字段值:%s!'
        print fmt_str % (FirstColVal, ParsePropertyName, ParseTargetType, ParsePropertyValue)
        break


f = file('bytes\\UIFrame.bytes', 'wb')
f.write(table.SerializeToString())
f.close()
