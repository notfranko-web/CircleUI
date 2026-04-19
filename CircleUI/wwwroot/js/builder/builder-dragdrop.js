(function(b) {
    document.addEventListener('dragstart', e => {
        const card = e.target.closest('.component-card[draggable]');
        if (card) {
            b.draggedComponentId = card.dataset.componentId;
            e.dataTransfer.effectAllowed = 'copy';
            console.log("Drag started (component):", b.draggedComponentId);
            return;
        }

        const asset = e.target.closest('.asset-item[draggable]');
        if (asset) {
            b.draggedAssetPath = asset.dataset.path;
            e.dataTransfer.effectAllowed = 'copy';
            console.log("Drag started (asset):", b.draggedAssetPath);
        }
    });

    document.addEventListener('dragend', e => {
        b.draggedComponentId = null;
        b.draggedAssetPath = null;
        console.log("Drag ended");
    });

    document.addEventListener('dragover', e => {
        const zone = e.target.closest('.section-drop-zone');
        if (zone) {
            e.preventDefault();
            e.dataTransfer.dropEffect = 'copy';
            zone.classList.add('drag-over');
        }
    });

    document.addEventListener('dragleave', e => {
        const zone = e.target.closest('.section-drop-zone');
        if (zone && !zone.contains(e.relatedTarget)) {
            zone.classList.remove('drag-over');
        }
    });

    document.addEventListener('drop', async e => {
        const zone = e.target.closest('.section-drop-zone');
        if (zone) {
            e.preventDefault();
            zone.classList.remove('drag-over');
            
            if (!b.draggedComponentId && !b.draggedAssetPath) return;

            const sectionId = zone.dataset.sectionId;
            const token = b.getAntiForgeryToken();
            if (!token) return;

            let res;
            if (b.draggedComponentId) {
                console.log("Drop component:", b.draggedComponentId, "to section:", sectionId);
                res = await fetch('/Section/AddComponent', {
                    method: 'POST',
                    headers: { 
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': token
                    },
                    body: `sectionId=${encodeURIComponent(sectionId)}&componentId=${encodeURIComponent(b.draggedComponentId)}`
                });
            } else if (b.draggedAssetPath) {
                console.log("Drop asset:", b.draggedAssetPath, "to section:", sectionId);
                res = await fetch('/Section/AddImageComponent', {
                    method: 'POST',
                    headers: { 
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': token
                    },
                    body: `sectionId=${encodeURIComponent(sectionId)}&imageUrl=${encodeURIComponent(b.draggedAssetPath)}`
                });
            }
            if (!res.ok) {
                console.error("Fetch failed with status:", res.status);
                return;
            }
            const data = await res.json();
            console.log("Component added result:", data);
            if (!data.success) {
                console.error("Component addition failed:", data.message);
                return;
            }

            function buildComponentEl(scId, type, componentId, content) {
                const el = document.createElement('div');
                el.className = 'border rounded p-2 bg-white small component-instance';
                el.dataset.scId = scId;

                const hdr = document.createElement('div');
                hdr.className = 'd-flex justify-content-between align-items-center mb-1';
                hdr.innerHTML = `<span class="badge bg-secondary">${type}</span>`;
                const btnGroup = document.createElement('div');
                btnGroup.className = 'd-flex align-items-center gap-1';
                if (type === 'Navbar') {
                    const addLinkBtn = document.createElement('button');
                    addLinkBtn.type = 'button';
                    addLinkBtn.className = 'btn btn-link text-primary p-0 add-nav-link-btn';
                    addLinkBtn.title = 'Add nav link';
                    addLinkBtn.textContent = '+ Link';
                    btnGroup.appendChild(addLinkBtn);
                }
                const removeBtn = document.createElement('button');
                removeBtn.type = 'button';
                removeBtn.className = 'btn btn-link text-danger p-0 remove-component-btn';
                removeBtn.title = 'Remove component';
                removeBtn.textContent = '×';
                btnGroup.appendChild(removeBtn);
                hdr.appendChild(btnGroup);
                el.appendChild(hdr);

                const contentDiv = document.createElement('div');
                contentDiv.className = 'component-editable';
                contentDiv.dataset.componentId = componentId;
                contentDiv.innerHTML = content;
                el.appendChild(contentDiv);
                return { el, contentDiv };
            }

            function appendToZone(targetZone, el, contentDiv) {
                const container = targetZone.querySelector('.section-components');
                const placeholder = container.querySelector('.drop-placeholder');
                if (placeholder) placeholder.remove();
                container.appendChild(el);
                if (contentDiv.querySelector('.carousel')) {
                    contentDiv.querySelectorAll('.carousel').forEach(c => new bootstrap.Carousel(c));
                }
                b.activateComponentEditing(contentDiv);
            }

            const { el, contentDiv } = buildComponentEl(data.sectionComponentId, data.type, data.componentId, data.content);
            appendToZone(zone, el, contentDiv);

            // If this is a global section, sync all other panes showing the same section
            if (zone.classList.contains('global-section')) {
                document.querySelectorAll(`.section-drop-zone[data-section-id="${sectionId}"]`).forEach(otherZone => {
                    if (otherZone === zone) return;
                    const { el: elClone, contentDiv: cdClone } = buildComponentEl(data.sectionComponentId, data.type, data.componentId, data.content);
                    appendToZone(otherZone, elClone, cdClone);
                });
            }

            console.log("New component element added and activated.");
        }
    });
})(window.builder);