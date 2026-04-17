window.builder = {
    projectId: null,
    activeTab: null,
    draggedComponentId: null,
    draggedAssetPath: null,
    activeImageElement: null,
    activeLinkElement: null,

    getAntiForgeryToken: function() {
        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        if (!tokenInput) {
            console.error("Anti-forgery token not found.");
            return '';
        }
        return tokenInput.value;
    },

    getLightboxModal: function() {
        return bootstrap.Modal.getOrCreateInstance(document.getElementById('lightboxModal'));
    },

    getImageSelectModal: function() {
        return bootstrap.Modal.getOrCreateInstance(document.getElementById('imageSelectModal'));
    },

    getLinkModal: function() {
        return bootstrap.Modal.getOrCreateInstance(document.getElementById('linkModal'));
    },

    init: function(config) {
        this.projectId = config.projectId;
        this.activeTab = config.activeTab;

        window.addEventListener('DOMContentLoaded', () => {
            const params = new URLSearchParams(window.location.search);
            let activeTab = params.get('activeTab') || this.activeTab;
            console.log("Activating tab:", activeTab);
            if (activeTab && activeTab !== 'home' && activeTab !== 'pane-') {
                const triggerEl = document.querySelector(`[data-bs-target="#${activeTab}"]`);
                if (triggerEl) {
                    console.log("Found trigger for tab:", activeTab);
                    triggerEl.click();
                } else {
                    console.warn("Could not find trigger for tab:", activeTab);
                }
            }
            
            this.loadAssets();
        });

        document.getElementById('assets-tab').addEventListener('shown.bs.tab', () => {
            console.log("Assets tab shown, reloading assets...");
            this.loadAssets();
        });
    }
};