# -*- coding: GB2312 -*-
import AvatarAsset_pb2
import codecs

def addItem(item, args):
	item.id = int(args[0])
	item.aname = args[1]
	item.pngpath = args[2]
	item.xmlpath = args[3]
	item.jsonpath = args[4]
	

table = AvatarAsset_pb2.AvatarAssetTable()
table.tname = 'AvatarAsset'
rf = codecs.open('E:\\cocoswork\\protoc-2.5.0-win32\\txt\\AvatarAsset.txt', encoding='GB2312')
rf.readline()
rf.readline()
rf.readline()
while True:
    line = rf.readline()
    if (len(line) == 0):break
    arr = line.replace('\r', '\n').split('\n')[0].split('\t')
    addItem(table.tlist.add(), arr)


f = file('E:\\cocoswork\\protoc-2.5.0-win32\\dbp\\AvatarAsset.dbp', 'wb')
f.write(table.SerializeToString())
f.close()

ddd = table.SerializeToString()
aaa = AvatarAsset_pb2.AvatarAssetTable()
aaa.ParseFromString(ddd)
print ddd
print aaa
