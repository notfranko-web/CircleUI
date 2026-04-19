(function(b) {
    b.activateComponentEditing = function(containerEl) {
        if (!containerEl) return;
        if (containerEl.dataset.editingActivated) return;
        containerEl.dataset.editingActivated = 'true';

        console.log("Activating editing for container:", containerEl);
        
        if (containerEl.querySelector('.carousel')) {
             console.log("Skipping editing activation for carousel container");
             return;
        }

        containerEl.querySelectorAll('[data-text-editable]').forEach(el => {
            el.removeAttribute('contenteditable');
            el.removeAttribute('spellcheck');
            el.removeAttribute('data-text-editable');
        });

        containerEl.querySelectorAll('div').forEach(el => {
            if (el.innerHTML.trim() === '' || el.innerHTML.trim() === '<br>') el.remove();
        });

        const walker = document.createTreeWalker(containerEl, NodeFilter.SHOW_TEXT);
        const seen = new Set();
        let node;
        while ((node = walker.nextNode())) {
            if (!node.textContent.trim()) continue;
            const parent = node.parentElement;
            if (!parent || parent === containerEl || seen.has(parent)) continue;
            
            if (parent.tagName === 'IMG' || parent.closest('img')) continue;

            seen.add(parent);
            parent.contentEditable = 'true';
            parent.spellcheck = false;
            parent.setAttribute('data-text-editable', '');
        }
        
        b.wireImagesInContainer(containerEl);
        b.wireLinksInContainer(containerEl);
    };

    b.saveComponentContent = async function(containerEl) {
        if (!containerEl) return;
        const componentId = containerEl.dataset.componentId;
        const clone = containerEl.cloneNode(true);
        clone.querySelectorAll('[data-text-editable]').forEach(el => {
            el.removeAttribute('contenteditable');
            el.removeAttribute('spellcheck');
            el.removeAttribute('data-text-editable');
            el.removeAttribute('data-editing-activated');
        });
        clone.querySelectorAll('img').forEach(img => {
            img.removeAttribute('contenteditable');
            img.removeAttribute('draggable');
        });
        const token = b.getAntiForgeryToken();
        const body = `componentId=${encodeURIComponent(componentId)}&content=${encodeURIComponent(clone.innerHTML)}`;
        
        await fetch('/Section/UpdateComponentContent', {
            method: 'POST',
            headers: { 
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token
            },
            body: body
        });
    };

    document.addEventListener('blur', async e => {
        const editable = e.target.closest('[data-text-editable]');
        if (editable) {
            const containerEl = editable.closest('.component-editable');
            if (!containerEl) return;
            const componentId = containerEl.dataset.componentId;

            const clone = containerEl.cloneNode(true);
            clone.querySelectorAll('[data-text-editable]').forEach(el => {
                el.removeAttribute('contenteditable');
                el.removeAttribute('spellcheck');
                el.removeAttribute('data-text-editable');
                el.removeAttribute('data-editing-activated');
            });
            clone.querySelectorAll('img').forEach(img => {
                img.removeAttribute('contenteditable');
                img.removeAttribute('draggable');
            });
            const token = b.getAntiForgeryToken();
            if (!token) return;

            console.log("Saving content for component:", componentId);
            const body = `componentId=${encodeURIComponent(componentId)}&content=${encodeURIComponent(clone.innerHTML)}`;
            const res = await fetch('/Section/UpdateComponentContent', {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token
                },
                body: body
            });
            if (!res.ok) {
                console.error("Content update failed with status:", res.status);
            } else {
                console.log("Content saved successfully.");
            }
        }
    }, true);

    document.addEventListener('click', async e => {
        const btn = e.target.closest('.remove-component-btn');
        if (btn) {
            const instance = btn.closest('.component-instance');
            const scId = instance.dataset.scId;
            console.log("Removing component instance:", scId);
            const token = b.getAntiForgeryToken();
            if (!token) return;

            const res = await fetch('/Section/RemoveComponent', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'RequestVerificationToken': token
                },
                body: `sectionComponentId=${encodeURIComponent(scId)}`
            });
            if (!res.ok) {
                console.error("Removal failed with status:", res.status);
                return;
            }
            const json = await res.json();
            if (json.success) {
                // Remove all DOM copies with this scId (covers global header/footer duplicated across page panes)
                document.querySelectorAll(`.component-instance[data-sc-id="${scId}"]`).forEach(el => {
                    const container = el.closest('.section-components');
                    el.remove();
                    if (container && !container.querySelector('.component-instance')) {
                        container.innerHTML = '<div class="drop-placeholder text-muted small fst-italic p-2 text-center border border-dashed rounded">Drop a component here</div>';
                    }
                });
            }
        }
    });
    window.addEventListener('DOMContentLoaded', () => {
        document.querySelectorAll('.component-editable').forEach(b.activateComponentEditing);
    });
})(window.builder);