import 'dart:io';

import 'package:flutter/material.dart';

class Utils {
  static Directory docsDir;

  static Future<String> pickDate(BuildContext context, String inDate) async {
    print(inDate);
    DateTime initialDate = DateTime.now();
    if (inDate != null) {
      final array = inDate.split("-");
      initialDate = new DateTime(int.parse(array[0]), int.parse(array[1]), int.parse(array[2]));
    }
    final dateTime = await showDatePicker(
        context: context,
        initialDate: initialDate,
        firstDate: DateTime(1970),
        lastDate: DateTime(2030)
    );
    if (dateTime != null)
      return "${dateTime.year}-${dateTime.month}-${dateTime.day}";
    else return "${initialDate.year}-${initialDate.month}-${initialDate.day}";
  }
}
