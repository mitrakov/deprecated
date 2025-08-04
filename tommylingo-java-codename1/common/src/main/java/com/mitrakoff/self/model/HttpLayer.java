package com.mitrakoff.self.model;

import com.codename1.io.ConnectionRequest;
import com.codename1.io.Util;
import com.codename1.util.StringUtil;
import com.codename1.util.SuccessCallback;
import com.codename1.xml.*;
import java.io.*;
import java.util.*;
import static com.codename1.ui.CN.*;

public class HttpLayer {
    private static final String URL = "https://mitrakoff.com/lingo";
    private final XMLParser parser = new XMLParser();
    private final XMLWriter writer = new XMLWriter(true);

    public void callFetch(String language, SuccessCallback<List<KeyTranslation>> callback) {
        /* response example:
        <?xml version='1.0' encoding='UTF-8'?>
        <result>
            <item key="rod">удочка</item>
            ...
        </result>
        */
        final ConnectionRequest request = new ConnectionRequest(StringUtil.join(new String[] {URL, "all", language}, "/")) {
            private final List<KeyTranslation> result = new ArrayList<>(2048);
            @Override
            protected void readResponse(InputStream input) {
                try {
                    String content = Util.readToString(input, "UTF-8");
                    System.out.println("==========================");
                    System.out.println(content);
                    System.out.println("==========================");
                } catch (IOException e) {
                    e.printStackTrace();
                } finally {
                    Util.cleanup(input);
                }

//                final Element root = parser.parse();
//                for (Object o : root.getDescendantsByTagName("item")) {
//                    result.add(new KeyTranslation(((Element) o).getAttribute("key"), ((Element) o).getChildAt(0).getText()));
//                }
            }
            @Override
            protected void postResponse() {
                callback.onSucess(result);
            }
        };
        request.setHttpMethod("GET");
        request.addRequestHeader("Authorization", "bearer 555");
        request.setInsecure(true);
        addToQueue(request);
    }

    public void callUpsert(String language, String key, String translation) {
        /* payload example:
        <?xml version="1.0"?>
        <a>
          <langCode>en-GB</langCode>
          <key>apple</key>
          <translation>яблоко</translation>
        </a>
         */
        // do not create anonymous classes with {{}}
        final Element c1 = new Element("langCode");
        c1.setText(language);
        final Element c2 = new Element("key");
        c2.setText(key);
        final Element c3 = new Element("translation");
        c3.setText(translation);
        final Element a = new Element("a");
        a.addChild(c1);
        a.addChild(c2);
        a.addChild(c3);
        final String xml = writer.toXML(a);
        final ConnectionRequest request = new ConnectionRequest(URL);
        request.setHttpMethod("POST");
        request.setRequestBody(xml);
        addToQueueAndWait(request);
    }

    public void callDelete(String language, String key) {
        /* payload example:
        <?xml version="1.0"?>
        <a>
          <langCode>en-GB</langCode>
          <key>apple</key>
        </a>
         */
        // do not create anonymous classes with {{}}
        final Element c1 = new Element("langCode");
        c1.setText(language);
        final Element c2 = new Element("key");
        c2.setText(key);
        final Element a = new Element("a");
        a.addChild(c1);
        a.addChild(c2);
        final String xml = writer.toXML(a);
        final ConnectionRequest request = new ConnectionRequest(URL);
        request.setHttpMethod("DELETE");
        request.setRequestBody(xml);
        addToQueueAndWait(request);
    }
}
