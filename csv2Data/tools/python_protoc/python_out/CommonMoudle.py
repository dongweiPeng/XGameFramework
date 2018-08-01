# -*- coding: UTF-8 -*-
#-------------------------------------------------------------------------------
# Name:        通用模块
# Purpose:
#
# Author:      Asvo
#
# Created:     29/03/2016
# Copyright:   (c) Administrator 2016
# Licence:     <your licence>
#-------------------------------------------------------------------------------

import xlrd

#设置值，含默认值处理
def setProtoValuePyExcel(targetType, value, isEnum):
    #枚举处理
    if isEnum:
        if isStringHasContent(value):
            return targetType.Value(value)
        else:
            return targetType.Name(0)
    if isinstance(value, str):
        #如果目标类型是bool
        if targetType == bool:
            if isStringHasContent(value):
                if not value == '0':
                    return True
                else:
                    return False
            else:
                return False
        if targetType ==str:
            #如果目标类型就是str
            if value.find(',') != -1:
                print u'DataError: has ,: %s ,' % value
            return value##.encode('utf-8')
        else:
            #否则就是其它类型. int , float等
            return targetType(0)

    #空处理
    if isStringHasContent(value) == True:
        #bool
        if targetType == bool:
            int_val = 0
            try:
                int_val = int(value)
            except Exception,e:
                int_val = 1 #如果不是空字符串，又不能强转换成int，认为它是bool的True值
            finally:
                return bool(int_val)
        if isinstance(value, unicode):
            if targetType == float or targetType == int:
                return targetType(value)
            else:
                if targetType == str:
                    x = value.encode('utf-8')
                    x = x.replace('\\n','\x0a')
                    if x.find(',') != -1:
                        print u'DataError: has ,: %s ,' % x
                return x
        if targetType == str:
            value_str = str(value)
            if value_str.find(',') != -1:
                print u'DataError: has ,: %s O,' % value_str
            return value_str.replace('.0', '')  #这里处理目标是string,填的确实数字的情况。注意，如果填的有小数，不能是xx.0xx这样。。否则数据出错
        return targetType(value)
    else:
        if targetType == str:
            return ''
        return targetType(0)



#设置值，含默认值处理
#如果是bool类型，传入''或者为0就是False
def setProtoValue(valueType, value, isEnum):
    if valueType == str:
        ##（py-excel处理）#这里不判断、转类型的话，google-protobuf在addItem的时候会抛出类型异常
        ##if isinstance(value, int) or isinstance(value, float):
        ##    return str(value)
        #字符串直接返回
        return value.encode('utf-8')
    #enum类型处理
    if isEnum:
        if isStringHasContent(value):
            return valueType.Value(value)
        else:
            return valueType.Name(0)

    if not value.strip() == "":
        #bool类型特殊处理
        if valueType == bool:
            int_val = 0
            try:
                int_val = int(value)
            except Exception,e:
                int_val = 1 #如果不是空字符串，又不能强转换成int，认为它是bool的True值
            finally:
                return bool(int_val)
        return valueType(value)
    else:
        return valueType(0)

#判断字符串有无内容。去除头尾空格
def isStringHasContent(testStr):
    if testStr == u'':
        return False
    if not isinstance(testStr, str):
        return True
    return True

def open_excel(filename):
    try:
        data = xlrd.open_workbook(filename)
        return data
    except Exception,e:
        print(e)
