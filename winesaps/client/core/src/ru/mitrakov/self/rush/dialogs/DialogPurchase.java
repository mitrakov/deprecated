package ru.mitrakov.self.rush.dialogs;

import com.badlogic.gdx.utils.*;
import com.badlogic.gdx.graphics.g2d.*;
import com.badlogic.gdx.assets.AssetManager;
import com.badlogic.gdx.scenes.scene2d.ui.*;
import com.badlogic.gdx.scenes.scene2d.Actor;
import com.badlogic.gdx.scenes.scene2d.utils.*;

import java.util.List;
import java.util.Collections;

import ru.mitrakov.self.rush.*;
import ru.mitrakov.self.rush.ui.*;

/**
 * "Choose SKU to purchase" dialog
 * @author Mitrakov
 */
public class DialogPurchase extends DialogFeat {
    /** Error label (only in case of errors) */
    private final Label lblError;

    /**
     * Creates a new "Choose SKU to purchase" dialog, where a user can tap on a SKU (e.g. "Big Gems Pack") and buy it
     * @param skin LibGdx skin
     * @param windowStyleName style name (usually just "default")
     * @param i18n LibGdx internationalization bundle
     */
    public DialogPurchase(Skin skin, String windowStyleName, I18NBundle i18n) {
        super("", skin, windowStyleName, true);
        Table table = getContentTable();
        assert i18n != null && table != null;

        table.pad(20).add(lblError = new Label(i18n.format("dialog.gems.error"), skin));
        button("OK"); // text will be replaced in onLocaleChanged()
    }

    @Override
    public void onLocaleChanged(I18NBundle bundle) {
        Table table = getContentTable();
        assert bundle != null && table != null;

        lblError.setText(bundle.format("dialog.gems.error"));

        // gems count info ("10 gems", "35 gems", etc.)
        for (Actor actor : table.getChildren()) {
            if (actor instanceof Label && actor.getUserObject() instanceof IBillingProvider.Sku) {
                Label lbl = ((Label) actor);
                IBillingProvider.Sku sku = (IBillingProvider.Sku) actor.getUserObject();
                lbl.setText(bundle.format("dialog.gems.count", sku.value));
            }
        }

        // close button
        if (getButtonTable() != null) {
            Array<Actor> buttons = getButtonTable().getChildren();
            assert buttons != null;
            if (buttons.size == 1) {
                Actor actor = buttons.first();
                if (actor instanceof TextButton) // stackoverflow.com/questions/2950319
                    ((TextButton) actor).setText(bundle.format("close"));
            }
        }
    }

    /**
     * Updates SKU images depending on the response of current Billing Provider
     * @param provider Billing Provider
     * @param username user name
     * @param i18n LibGdx internationalization bundle
     * @param assetManager LibGdx Assets Manager
     * @param audioManager {@link AudioManager}
     */
    public void updateSkuButtons(final IBillingProvider provider, final String username, I18NBundle i18n,
                                 AssetManager assetManager, AudioManager audioManager) {
        assert provider != null && i18n != null && assetManager != null && audioManager != null;

        TextureAtlas atlas = assetManager.get("pack/menu.pack");
        Skin skin = assetManager.get("skin/uiskin.json");

        Table table = getContentTable();
        assert table != null;
        table.clear();

        List<IBillingProvider.Sku> products = provider.getProducts();
        Collections.sort(products);

        for (final IBillingProvider.Sku sku : products) {
            TextureRegion texture = atlas.findRegion(sku.id);
            if (texture != null) {
                table.add(new ImageButtonFeat(new TextureRegionDrawable(texture), audioManager, new Runnable() {
                    @Override
                    public void run() {
                        provider.purchaseProduct(sku, username);
                    }
                })).spaceLeft(20);
            }
        }

        table.row();
        for (IBillingProvider.Sku sku : products) {
            Label lbl = new Label(i18n.format("dialog.gems.count", sku.value), skin);
            lbl.setUserObject(sku);
            table.add(lbl).spaceLeft(20);
        }

        table.row();
        for (IBillingProvider.Sku sku : products) {
            table.add(new Label(sku.price, skin)).spaceLeft(20);
        }

        pack();
    }
}
