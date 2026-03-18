function mettreAJourApercuCertificat() {
    const zone = document.getElementById("zone-certificat");
    const certificat = document.getElementById("contenu-a-imprimer");

    if (!zone || !certificat || zone.style.display === "none") return;

    certificat.style.transform = "scale(1)";

    const largeurDisponible = Math.max(window.innerWidth - 40, 320);
    const largeurCertificat = certificat.offsetWidth;
    const hauteurCertificat = certificat.offsetHeight;
    const echelle = Math.min(1, largeurDisponible / largeurCertificat);

    certificat.style.transform = `scale(${echelle})`;
    zone.style.height = `${hauteurCertificat * echelle}px`;
}

function preparerDiplome() {
    const prenom = document.getElementById("input-prenom").value.trim();
    const nom = document.getElementById("input-nom").value.trim();

    if (prenom === "" || nom === "") {
        alert("Erreur système : Identité requise ! N'oublie pas ton nom et prénom.");
        return;
    }

    let scoreTotal = localStorage.getItem("brainhack_score");
    if (!scoreTotal) scoreTotal = 1500;

    document.getElementById("affichage-nom").innerText = prenom + " " + nom;
    document.getElementById("affichage-xp").innerText = scoreTotal;

    document.getElementById("formulaire-section").style.display = "none";
    document.getElementById("zone-certificat").style.display = "block";
    document.getElementById("bouton-pdf").style.display = "block";

    mettreAJourApercuCertificat();
}

async function telechargerPDF() {
    const zone = document.getElementById("zone-certificat");
    const element = document.getElementById("contenu-a-imprimer");
    const boutonPdf = document.getElementById("bouton-pdf");
    const prenom = document.getElementById("input-prenom").value.toUpperCase().replace(/\s+/g, "_");
    const nom = document.getElementById("input-nom").value.toUpperCase().replace(/\s+/g, "_");
    const nomFichier = `BrainHack_Certification_${prenom}_${nom}.pdf`;

    if (!window.html2pdf) {
        alert("Le module PDF n'est pas chargé. Recharge la page puis réessaie.");
        return;
    }

    const ancienneTransformation = element.style.transform;
    const ancienneHauteurZone = zone.style.height;
    const ancienTexteBouton = boutonPdf.innerText;

    element.style.transform = "scale(1)";
    element.classList.add("export-pdf");
    zone.style.height = "auto";
    boutonPdf.disabled = true;
    boutonPdf.innerText = "⏳ Génération du PDF...";

    try {
        await window.html2pdf().set({
            margin: 0,
            filename: nomFichier,
            image: { type: "jpeg", quality: 1 },
            html2canvas: {
                scale: 2,
                useCORS: true,
                backgroundColor: "#ffffff",
                width: 1122,
                height: 793,
            },
            jsPDF: {
                orientation: "landscape",
                unit: "mm",
                format: "a4",
            },
            pagebreak: { mode: ["avoid-all"] },
        })
        .from(element)
        .save();
    } catch (error) {
        alert("Erreur pendant la génération du PDF. Recharge la page puis réessaie.");
    } finally {
        element.style.transform = ancienneTransformation;
        element.classList.remove("export-pdf");
        zone.style.height = ancienneHauteurZone;
        boutonPdf.disabled = false;
        boutonPdf.innerText = ancienTexteBouton;
        mettreAJourApercuCertificat();
    }
}

window.addEventListener("resize", mettreAJourApercuCertificat);
