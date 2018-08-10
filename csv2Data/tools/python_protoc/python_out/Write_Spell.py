# -*- coding: utf-8 -*-
import Spell_pb2
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
	ParsePropertyName = 'Name'
	ParsePropertyValue = args[1]
	ParseTargetType = 'str' 
	item.Name = CommonMoudle.setProtoValuePyExcel(str, args[1], False)
	ParsePropertyName = 'Star'
	ParsePropertyValue = args[2]
	ParseTargetType = 'int' 
	item.Star = CommonMoudle.setProtoValuePyExcel(int, args[2], False)
	ParsePropertyName = 'DmgRate'
	ParsePropertyValue = args[3]
	ParseTargetType = 'int' 
	item.DmgRate = CommonMoudle.setProtoValuePyExcel(int, args[3], False)
	ParsePropertyName = 'Damage'
	ParsePropertyValue = args[4]
	ParseTargetType = 'int' 
	item.Damage = CommonMoudle.setProtoValuePyExcel(int, args[4], False)
	ParsePropertyName = 'Hit'
	ParsePropertyValue = args[5]
	ParseTargetType = 'int' 
	item.Hit = CommonMoudle.setProtoValuePyExcel(int, args[5], False)
	ParsePropertyName = 'Block'
	ParsePropertyValue = args[6]
	ParseTargetType = 'int' 
	item.Block = CommonMoudle.setProtoValuePyExcel(int, args[6], False)
	ParsePropertyName = 'Dodge'
	ParsePropertyValue = args[7]
	ParseTargetType = 'int' 
	item.Dodge = CommonMoudle.setProtoValuePyExcel(int, args[7], False)
	ParsePropertyName = 'SkillText1'
	ParsePropertyValue = args[8]
	ParseTargetType = 'str' 
	item.SkillText1 = CommonMoudle.setProtoValuePyExcel(str, args[8], False)
	ParsePropertyName = 'SkillText2'
	ParsePropertyValue = args[9]
	ParseTargetType = 'str' 
	item.SkillText2 = CommonMoudle.setProtoValuePyExcel(str, args[9], False)
	ParsePropertyName = 'SkillText3'
	ParsePropertyValue = args[10]
	ParseTargetType = 'str' 
	item.SkillText3 = CommonMoudle.setProtoValuePyExcel(str, args[10], False)
	ParsePropertyName = 'SkillText4'
	ParsePropertyValue = args[11]
	ParseTargetType = 'str' 
	item.SkillText4 = CommonMoudle.setProtoValuePyExcel(str, args[11], False)
	ParsePropertyName = 'SkillName'
	ParsePropertyValue = args[12]
	ParseTargetType = 'str' 
	item.SkillName = CommonMoudle.setProtoValuePyExcel(str, args[12], False)
	

table = Spell_pb2.SpellTable()
table.tname = 'Spell.bytes'
rf = CommonMoudle.open_excel('txt\\������.xlsx')
sheet = rf.sheet_by_name('Spell')
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


f = file('bytes\\Spell.bytes', 'wb')
f.write(table.SerializeToString())
f.close()
