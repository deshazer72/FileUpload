function downloadFile(filename) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "/File/Download/" + filename;
    link.click();
}