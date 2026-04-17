(function(b) {
    b.loadAssets = async function() {
        console.log("Loading assets...");
        const res = await fetch('/Asset/GetMyAssets');
        if (!res.ok) {
            console.error("Failed to load assets:", res.status);
            return;
        }
        const assets = await res.json();
        console.log("Assets loaded:", assets.length);
        
        const assetList = document.getElementById('assetList');
        const modalAssetList = document.getElementById('modalAssetList');
        
        const html = assets.map(a => `
            <div class="asset-item-container">
                <img src="${a.storagePath}" class="asset-item img-fluid rounded" title="${a.fileName}" data-path="${a.storagePath}" draggable="true">
                <button class="btn btn-danger btn-sm delete-asset-btn" onclick="builder.deleteAsset('${a.id}', event)">×</button>
            </div>`).join('');
        const modalHtml = assets.map(a => `
            <div class="asset-item-container">
                <img src="${a.storagePath}" class="modal-asset-item img-fluid rounded" title="${a.fileName}" data-path="${a.storagePath}">
                <button class="btn btn-danger btn-sm delete-asset-btn" onclick="builder.deleteAsset('${a.id}', event)">×</button>
            </div>`).join('');
        
        if (assetList) assetList.innerHTML = html || '<div class="text-muted small w-100 text-center py-2">No photos uploaded.</div>';
        if (modalAssetList) modalAssetList.innerHTML = modalHtml || '<div class="text-muted small w-100 text-center py-2">No photos uploaded.</div>';
    };

    b.deleteAsset = async function(assetId, event) {
        if (event) {
            event.preventDefault();
            event.stopPropagation();
        }
        if (!confirm('Are you sure you want to delete this asset?')) return;

        const token = b.getAntiForgeryToken();
        try {
            const res = await fetch(`/Asset/Delete/${assetId}`, {
                method: 'POST',
                headers: { 'RequestVerificationToken': token }
            });
            const json = await res.json();
            if (json.success) {
                await b.loadAssets();
            } else {
                alert(json.message || "Delete failed");
            }
        } catch (err) {
            console.error(err);
        }
    };

    b.uploadFile = async function(file, isModal = false) {
        const progress = document.getElementById('uploadProgress');
        if (progress && !isModal) progress.classList.remove('d-none');
        
        const formData = new FormData();
        formData.append('file', file);
        
        const token = b.getAntiForgeryToken();
        
        try {
            const res = await fetch('/Asset/Upload', {
                method: 'POST',
                headers: { 'RequestVerificationToken': token },
                body: formData
            });
            const json = await res.json();
            if (json.success) {
                await b.loadAssets();
            } else {
                alert(json.message || "Upload failed");
            }
        } catch (err) {
            console.error(err);
        } finally {
            if (progress && !isModal) progress.classList.add('d-none');
        }
    };

    window.addEventListener('DOMContentLoaded', () => {
        const uploadInput = document.getElementById('assetUploadInput');
        if (uploadInput) {
            uploadInput.addEventListener('change', e => {
                if (e.target.files.length > 0) b.uploadFile(e.target.files[0]);
            });
        }
        
        const modalUploadInput = document.getElementById('modalAssetUploadInput');
        if (modalUploadInput) {
            modalUploadInput.addEventListener('change', e => {
                if (e.target.files.length > 0) b.uploadFile(e.target.files[0], true);
            });
        }

    });

    document.addEventListener('click', function(e) {
        if (e.target.closest('.remove-component-btn')) return;

        const modalAsset = e.target.closest('.modal-asset-item');
        if (modalAsset) {
            if (b.activeImageElement === 'PROJECT_BACKGROUND') {
                const path = modalAsset.dataset.path;
                document.getElementById('backgroundImage').value = path;
                document.getElementById('bgImagePreview').innerHTML = `<img src="${path}" style="width: 100%; height: 100%; object-fit: cover;" />`;
                document.documentElement.style.setProperty('--project-bg-image', `url('${path}')`);
                document.documentElement.style.setProperty('--project-effective-bg-color', 'transparent');
                b.getImageSelectModal().hide();
                b.activeImageElement = null;
                e.preventDefault();
                e.stopPropagation();
                return;
            } else if (b.activeImageElement) {
                b.activeImageElement.src = modalAsset.dataset.path;
                const container = b.activeImageElement.closest('.component-editable');
                if (container) b.saveComponentContent(container);
                b.getImageSelectModal().hide();
                e.preventDefault();
                e.stopPropagation();
                return;
            }
        }

        const asset = e.target.closest('.asset-item') || e.target.closest('.modal-asset-item');
        if (asset) {
            if (e.target.closest('.delete-asset-btn')) return;
            e.preventDefault();
            e.stopPropagation();
            b.openLightbox(asset, false);
        }
    });
})(window.builder);