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

            const container = zone.querySelector('.section-components');
            const placeholder = container.querySelector('.drop-placeholder');
            if (placeholder) placeholder.remove();

            const el = document.createElement('div');
            el.className = 'border rounded p-2 bg-white small component-instance';
            el.dataset.scId = data.sectionComponentId;

            const header = document.createElement('div');
            header.className = 'd-flex justify-content-between align-items-center mb-1';
            header.innerHTML = `<span class="badge bg-secondary">${data.type}</span>`;
            const removeBtn = document.createElement('button');
            removeBtn.type = 'button';
            removeBtn.className = 'btn btn-link text-danger p-0 remove-component-btn';
            removeBtn.title = 'Remove component';
            removeBtn.textContent = '×';
            header.appendChild(removeBtn);
            el.appendChild(header);

            const contentDiv = document.createElement('div');
            contentDiv.className = 'component-editable';
            contentDiv.dataset.componentId = data.componentId;
            contentDiv.innerHTML = data.content;

            el.appendChild(contentDiv);
            container.appendChild(el);
            
            if (contentDiv.querySelector('.carousel')) {
                const carousels = contentDiv.querySelectorAll('.carousel');
                carousels.forEach(c => {
                    new bootstrap.Carousel(c);
                });
            }

            b.activateComponentEditing(contentDiv);
            console.log("New component element added and activated.");
        }
    });
})(window.builder);