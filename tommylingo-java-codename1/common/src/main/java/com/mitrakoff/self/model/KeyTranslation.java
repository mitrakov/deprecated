package com.mitrakoff.self.model;

public class KeyTranslation {
    private final String key;
    private final String translation;

    public KeyTranslation(String key, String translation) {
        this.key = key;
        this.translation = translation;
    }

    public String getKey() {
        return key;
    }

    public String getTranslation() {
        return translation;
    }
}
