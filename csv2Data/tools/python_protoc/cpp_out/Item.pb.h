// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Item.proto

#ifndef PROTOBUF_Item_2eproto__INCLUDED
#define PROTOBUF_Item_2eproto__INCLUDED

#include <string>

#include <google/protobuf/stubs/common.h>

#if GOOGLE_PROTOBUF_VERSION < 2005000
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please update
#error your headers.
#endif
#if 2005000 < GOOGLE_PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/message.h>
#include <google/protobuf/repeated_field.h>
#include <google/protobuf/extension_set.h>
#include <google/protobuf/unknown_field_set.h>
// @@protoc_insertion_point(includes)

namespace dbc {

// Internal implementation detail -- do not call these.
void  protobuf_AddDesc_Item_2eproto();
void protobuf_AssignDesc_Item_2eproto();
void protobuf_ShutdownFile_Item_2eproto();

class ItemTable;
class Item;

// ===================================================================

class ItemTable : public ::google::protobuf::Message {
 public:
  ItemTable();
  virtual ~ItemTable();

  ItemTable(const ItemTable& from);

  inline ItemTable& operator=(const ItemTable& from) {
    CopyFrom(from);
    return *this;
  }

  inline const ::google::protobuf::UnknownFieldSet& unknown_fields() const {
    return _unknown_fields_;
  }

  inline ::google::protobuf::UnknownFieldSet* mutable_unknown_fields() {
    return &_unknown_fields_;
  }

  static const ::google::protobuf::Descriptor* descriptor();
  static const ItemTable& default_instance();

  void Swap(ItemTable* other);

  // implements Message ----------------------------------------------

  ItemTable* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const ItemTable& from);
  void MergeFrom(const ItemTable& from);
  void Clear();
  bool IsInitialized() const;

  int ByteSize() const;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input);
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const;
  ::google::protobuf::uint8* SerializeWithCachedSizesToArray(::google::protobuf::uint8* output) const;
  int GetCachedSize() const { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const;
  public:

  ::google::protobuf::Metadata GetMetadata() const;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // optional string tname = 1;
  inline bool has_tname() const;
  inline void clear_tname();
  static const int kTnameFieldNumber = 1;
  inline const ::std::string& tname() const;
  inline void set_tname(const ::std::string& value);
  inline void set_tname(const char* value);
  inline void set_tname(const char* value, size_t size);
  inline ::std::string* mutable_tname();
  inline ::std::string* release_tname();
  inline void set_allocated_tname(::std::string* tname);

  // repeated .dbc.Item tlist = 2;
  inline int tlist_size() const;
  inline void clear_tlist();
  static const int kTlistFieldNumber = 2;
  inline const ::dbc::Item& tlist(int index) const;
  inline ::dbc::Item* mutable_tlist(int index);
  inline ::dbc::Item* add_tlist();
  inline const ::google::protobuf::RepeatedPtrField< ::dbc::Item >&
      tlist() const;
  inline ::google::protobuf::RepeatedPtrField< ::dbc::Item >*
      mutable_tlist();

  // @@protoc_insertion_point(class_scope:dbc.ItemTable)
 private:
  inline void set_has_tname();
  inline void clear_has_tname();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::std::string* tname_;
  ::google::protobuf::RepeatedPtrField< ::dbc::Item > tlist_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(2 + 31) / 32];

  friend void  protobuf_AddDesc_Item_2eproto();
  friend void protobuf_AssignDesc_Item_2eproto();
  friend void protobuf_ShutdownFile_Item_2eproto();

  void InitAsDefaultInstance();
  static ItemTable* default_instance_;
};
// -------------------------------------------------------------------

class Item : public ::google::protobuf::Message {
 public:
  Item();
  virtual ~Item();

  Item(const Item& from);

  inline Item& operator=(const Item& from) {
    CopyFrom(from);
    return *this;
  }

  inline const ::google::protobuf::UnknownFieldSet& unknown_fields() const {
    return _unknown_fields_;
  }

  inline ::google::protobuf::UnknownFieldSet* mutable_unknown_fields() {
    return &_unknown_fields_;
  }

  static const ::google::protobuf::Descriptor* descriptor();
  static const Item& default_instance();

  void Swap(Item* other);

  // implements Message ----------------------------------------------

  Item* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const Item& from);
  void MergeFrom(const Item& from);
  void Clear();
  bool IsInitialized() const;

  int ByteSize() const;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input);
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const;
  ::google::protobuf::uint8* SerializeWithCachedSizesToArray(::google::protobuf::uint8* output) const;
  int GetCachedSize() const { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const;
  public:

  ::google::protobuf::Metadata GetMetadata() const;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // optional int32 id = 1;
  inline bool has_id() const;
  inline void clear_id();
  static const int kIdFieldNumber = 1;
  inline ::google::protobuf::int32 id() const;
  inline void set_id(::google::protobuf::int32 value);

  // optional string name = 2;
  inline bool has_name() const;
  inline void clear_name();
  static const int kNameFieldNumber = 2;
  inline const ::std::string& name() const;
  inline void set_name(const ::std::string& value);
  inline void set_name(const char* value);
  inline void set_name(const char* value, size_t size);
  inline ::std::string* mutable_name();
  inline ::std::string* release_name();
  inline void set_allocated_name(::std::string* name);

  // optional string price = 3;
  inline bool has_price() const;
  inline void clear_price();
  static const int kPriceFieldNumber = 3;
  inline const ::std::string& price() const;
  inline void set_price(const ::std::string& value);
  inline void set_price(const char* value);
  inline void set_price(const char* value, size_t size);
  inline ::std::string* mutable_price();
  inline ::std::string* release_price();
  inline void set_allocated_price(::std::string* price);

  // @@protoc_insertion_point(class_scope:dbc.Item)
 private:
  inline void set_has_id();
  inline void clear_has_id();
  inline void set_has_name();
  inline void clear_has_name();
  inline void set_has_price();
  inline void clear_has_price();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::std::string* name_;
  ::std::string* price_;
  ::google::protobuf::int32 id_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(3 + 31) / 32];

  friend void  protobuf_AddDesc_Item_2eproto();
  friend void protobuf_AssignDesc_Item_2eproto();
  friend void protobuf_ShutdownFile_Item_2eproto();

  void InitAsDefaultInstance();
  static Item* default_instance_;
};
// ===================================================================


// ===================================================================

// ItemTable

// optional string tname = 1;
inline bool ItemTable::has_tname() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void ItemTable::set_has_tname() {
  _has_bits_[0] |= 0x00000001u;
}
inline void ItemTable::clear_has_tname() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void ItemTable::clear_tname() {
  if (tname_ != &::google::protobuf::internal::kEmptyString) {
    tname_->clear();
  }
  clear_has_tname();
}
inline const ::std::string& ItemTable::tname() const {
  return *tname_;
}
inline void ItemTable::set_tname(const ::std::string& value) {
  set_has_tname();
  if (tname_ == &::google::protobuf::internal::kEmptyString) {
    tname_ = new ::std::string;
  }
  tname_->assign(value);
}
inline void ItemTable::set_tname(const char* value) {
  set_has_tname();
  if (tname_ == &::google::protobuf::internal::kEmptyString) {
    tname_ = new ::std::string;
  }
  tname_->assign(value);
}
inline void ItemTable::set_tname(const char* value, size_t size) {
  set_has_tname();
  if (tname_ == &::google::protobuf::internal::kEmptyString) {
    tname_ = new ::std::string;
  }
  tname_->assign(reinterpret_cast<const char*>(value), size);
}
inline ::std::string* ItemTable::mutable_tname() {
  set_has_tname();
  if (tname_ == &::google::protobuf::internal::kEmptyString) {
    tname_ = new ::std::string;
  }
  return tname_;
}
inline ::std::string* ItemTable::release_tname() {
  clear_has_tname();
  if (tname_ == &::google::protobuf::internal::kEmptyString) {
    return NULL;
  } else {
    ::std::string* temp = tname_;
    tname_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
    return temp;
  }
}
inline void ItemTable::set_allocated_tname(::std::string* tname) {
  if (tname_ != &::google::protobuf::internal::kEmptyString) {
    delete tname_;
  }
  if (tname) {
    set_has_tname();
    tname_ = tname;
  } else {
    clear_has_tname();
    tname_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  }
}

// repeated .dbc.Item tlist = 2;
inline int ItemTable::tlist_size() const {
  return tlist_.size();
}
inline void ItemTable::clear_tlist() {
  tlist_.Clear();
}
inline const ::dbc::Item& ItemTable::tlist(int index) const {
  return tlist_.Get(index);
}
inline ::dbc::Item* ItemTable::mutable_tlist(int index) {
  return tlist_.Mutable(index);
}
inline ::dbc::Item* ItemTable::add_tlist() {
  return tlist_.Add();
}
inline const ::google::protobuf::RepeatedPtrField< ::dbc::Item >&
ItemTable::tlist() const {
  return tlist_;
}
inline ::google::protobuf::RepeatedPtrField< ::dbc::Item >*
ItemTable::mutable_tlist() {
  return &tlist_;
}

// -------------------------------------------------------------------

// Item

// optional int32 id = 1;
inline bool Item::has_id() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void Item::set_has_id() {
  _has_bits_[0] |= 0x00000001u;
}
inline void Item::clear_has_id() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void Item::clear_id() {
  id_ = 0;
  clear_has_id();
}
inline ::google::protobuf::int32 Item::id() const {
  return id_;
}
inline void Item::set_id(::google::protobuf::int32 value) {
  set_has_id();
  id_ = value;
}

// optional string name = 2;
inline bool Item::has_name() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void Item::set_has_name() {
  _has_bits_[0] |= 0x00000002u;
}
inline void Item::clear_has_name() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void Item::clear_name() {
  if (name_ != &::google::protobuf::internal::kEmptyString) {
    name_->clear();
  }
  clear_has_name();
}
inline const ::std::string& Item::name() const {
  return *name_;
}
inline void Item::set_name(const ::std::string& value) {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  name_->assign(value);
}
inline void Item::set_name(const char* value) {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  name_->assign(value);
}
inline void Item::set_name(const char* value, size_t size) {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  name_->assign(reinterpret_cast<const char*>(value), size);
}
inline ::std::string* Item::mutable_name() {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  return name_;
}
inline ::std::string* Item::release_name() {
  clear_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    return NULL;
  } else {
    ::std::string* temp = name_;
    name_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
    return temp;
  }
}
inline void Item::set_allocated_name(::std::string* name) {
  if (name_ != &::google::protobuf::internal::kEmptyString) {
    delete name_;
  }
  if (name) {
    set_has_name();
    name_ = name;
  } else {
    clear_has_name();
    name_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  }
}

// optional string price = 3;
inline bool Item::has_price() const {
  return (_has_bits_[0] & 0x00000004u) != 0;
}
inline void Item::set_has_price() {
  _has_bits_[0] |= 0x00000004u;
}
inline void Item::clear_has_price() {
  _has_bits_[0] &= ~0x00000004u;
}
inline void Item::clear_price() {
  if (price_ != &::google::protobuf::internal::kEmptyString) {
    price_->clear();
  }
  clear_has_price();
}
inline const ::std::string& Item::price() const {
  return *price_;
}
inline void Item::set_price(const ::std::string& value) {
  set_has_price();
  if (price_ == &::google::protobuf::internal::kEmptyString) {
    price_ = new ::std::string;
  }
  price_->assign(value);
}
inline void Item::set_price(const char* value) {
  set_has_price();
  if (price_ == &::google::protobuf::internal::kEmptyString) {
    price_ = new ::std::string;
  }
  price_->assign(value);
}
inline void Item::set_price(const char* value, size_t size) {
  set_has_price();
  if (price_ == &::google::protobuf::internal::kEmptyString) {
    price_ = new ::std::string;
  }
  price_->assign(reinterpret_cast<const char*>(value), size);
}
inline ::std::string* Item::mutable_price() {
  set_has_price();
  if (price_ == &::google::protobuf::internal::kEmptyString) {
    price_ = new ::std::string;
  }
  return price_;
}
inline ::std::string* Item::release_price() {
  clear_has_price();
  if (price_ == &::google::protobuf::internal::kEmptyString) {
    return NULL;
  } else {
    ::std::string* temp = price_;
    price_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
    return temp;
  }
}
inline void Item::set_allocated_price(::std::string* price) {
  if (price_ != &::google::protobuf::internal::kEmptyString) {
    delete price_;
  }
  if (price) {
    set_has_price();
    price_ = price;
  } else {
    clear_has_price();
    price_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  }
}


// @@protoc_insertion_point(namespace_scope)

}  // namespace dbc

#ifndef SWIG
namespace google {
namespace protobuf {


}  // namespace google
}  // namespace protobuf
#endif  // SWIG

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_Item_2eproto__INCLUDED
