window.BlazorDownloadFile = (options) => {
    const byteArray = new Uint8Array(options.byteArray);
    const blob = new Blob([byteArray], { type: options.contentType });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    document.body.appendChild(a);
    a.href = url;
    a.download = options.fileName;
    a.click();
    window.URL.revokeObjectURL(url);
};