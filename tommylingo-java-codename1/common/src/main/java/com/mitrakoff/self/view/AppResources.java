package com.mitrakoff.self.view;

import com.codename1.ui.Image;
import java.io.IOException;

public class AppResources {
    public static final Image flagEnGb = loadImage("/en-gb.png");
    public static final Image flagEsEs = loadImage("/es-es.png");

    public static Image loadImage(String resourcesFilenameWithSlash) {
        try {
            return Image.createImage(resourcesFilenameWithSlash);
        } catch (IOException e) {
            throw new RuntimeException(e);
        }
    }
}
