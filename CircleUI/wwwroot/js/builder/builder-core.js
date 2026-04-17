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

        const saveThemeBtn = document.getElementById('saveThemeBtn');
        if (saveThemeBtn) {
            saveThemeBtn.addEventListener('click', () => this.saveTheme());
        }
    },

    saveTheme: async function() {
        const projectId = document.getElementById('themeProjectId').value;
        const backgroundColor = document.getElementById('backgroundColor').value;
        const primaryTextColor = document.getElementById('primaryTextColor').value;
        const secondaryTextColor = document.getElementById('secondaryTextColor').value;
        const buttonColor = document.getElementById('buttonColor').value;
        const buttonTextColor = document.getElementById('buttonTextColor').value;
        
        const status = document.getElementById('themeStatus');
        status.classList.remove('d-none', 'text-success', 'text-danger');
        status.textContent = 'Saving...';

        const token = this.getAntiForgeryToken();
        try {
            const formData = new URLSearchParams();
            formData.append('projectId', projectId);
            formData.append('backgroundColor', backgroundColor);
            formData.append('primaryTextColor', primaryTextColor);
            formData.append('secondaryTextColor', secondaryTextColor);
            formData.append('buttonColor', buttonColor);
            formData.append('buttonTextColor', buttonTextColor);

            const res = await fetch('/WebsiteProject/UpdateTheme', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token
                },
                body: formData.toString()
            });

            const data = await res.json();
            if (data.success) {
                status.textContent = 'Theme saved successfully!';
                status.classList.add('text-success');
                
                // Update CSS variables locally
                document.documentElement.style.setProperty('--project-bg-color', backgroundColor);
                document.documentElement.style.setProperty('--project-primary-text-color', primaryTextColor);
                document.documentElement.style.setProperty('--project-secondary-text-color', secondaryTextColor);
                document.documentElement.style.setProperty('--project-button-color', buttonColor);
                document.documentElement.style.setProperty('--project-button-text-color', buttonTextColor);
            } else {
                status.textContent = 'Error: ' + data.message;
                status.classList.add('text-danger');
            }
        } catch (err) {
            console.error(err);
            status.textContent = 'Error saving theme.';
            status.classList.add('text-danger');
        }
    }
};