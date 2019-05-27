function validateMyForm() {

    value = document.getElementById("amount").value;
    value = value.replace(/\./g, '').replace(',', '.');

    if (value.includes("-") || isNaN(value) || value <= 0) {

        var para = document.createElement("p");
        var node = document.createTextNode("Invalid amount, must be positive");
        para.id = "invalidAmount";
        if (document.getElementById("invalidAmount")) {
            return false;
        }

        para.appendChild(node);
        var element = document.getElementById("valLi");
        element.appendChild(para);
        return false;
    }
    return true;
}
