# -*- coding: GB2312 -*-
import python_out.AvatarAsset_pb2
import codecs
import os

bytes_path = os.getcwd() + '\\dbp\\AvatarAsset.dbp'

wer = python_out.AvatarAsset_pb2.AvatarAssetTable()

f = file(bytes_path, 'rb')
#f = codecs.open('E:\\cocoswork\\protoc-2.5.0-win32\\dbp\\AvatarAsset.dbp', encoding='UTF-8')

wer.ParseFromString(f.read())
print 'obj:', wer
f.close()
