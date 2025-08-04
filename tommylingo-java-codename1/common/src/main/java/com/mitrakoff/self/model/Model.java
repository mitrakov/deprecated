package com.mitrakoff.self.model;

import com.codename1.util.SuccessCallback;
import java.util.*;
import static java.util.Collections.emptyList;

@SuppressWarnings("Java8MapApi")
public class Model {
    public static final String enGB = "en-GB";
    public static final String esES = "es-ES";

    private final HttpLayer http = new HttpLayer();
    private final Map<String, List<KeyTranslation>> sourceOfTruth = new TreeMap<>();
    private final List<String> keys = new ArrayList<>();
    private final Map<String, String> dictionary = new HashMap<>();

    private String curLanguage = "";
    private int curPointer = 0;

    public void loadAll(SuccessCallback<Void> onFinish) {
        http.callFetch(enGB, lst -> {
            http.callFetch(esES, list -> sourceOfTruth.put(esES, list)); // do not call asynchronously: may produce bugs
            sourceOfTruth.put(enGB, lst);
            setLanguage(enGB);
            onFinish.onSucess(null);
        });
    }

    public List<String> getAllKeys() {
        return keys;
    }

    public String getCurrentLanguage() {
        return curLanguage;
    }

    public String getCurrentKey() {
        if (!keys.isEmpty()) return keys.get(curPointer);
        return "";
    }

    public String getCurrentTranslation() {
        final String key = getCurrentKey();
        return dictionary.containsKey(key) ? dictionary.get(key) : ""; // getOrDefault() is not available
    }

    public void setLanguage(String language) {
        if (!language.equals(curLanguage)) {
            curLanguage = language;
            curPointer = 0;
            keys.clear();
            dictionary.clear();
            final List<KeyTranslation> lst = sourceOfTruth.containsKey(language) ? sourceOfTruth.get(language) : emptyList(); // getOrDefault() is not available
            for (KeyTranslation p : lst) {
                keys.add(p.getKey());
                dictionary.put(p.getKey(), p.getTranslation());
            }
            Collections.shuffle(keys);
        }
    }

    public String next() {
        if (++curPointer >= keys.size()) { // if all words run out
            Collections.shuffle(keys);
            curPointer = 0;
        }
        return getCurrentKey();
    }

    public void upsert(String key, String translation) {
        http.callUpsert(curLanguage, key, translation);
    }

    public void delete(String key) {
        http.callDelete(curLanguage, key);
    }
}
