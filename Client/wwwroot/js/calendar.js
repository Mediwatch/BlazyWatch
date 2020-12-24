var url = "https://localhost:5001/formations/"

scheduler.config.readonly_form = true;
scheduler.config.drag_move = false;
scheduler.locale = {
    date: {
        month_full: ["Janvier", "Février", "Mars", "Avril", "Mai", "Juin", "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"],
        month_short: ["Jan", "Fév", "Mar", "Avr", "Mai", "Juin", "Juil", "Août", "Sept", "Oct", "Nov", "Déc"],
        day_full: ["Dimanche", "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi"],
        day_short: ["Dim", "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam"]
    },
    labels: {
        dhx_cal_today_button: "Aujourd'hui",
        day_tab: "Jour",
        week_tab: "Semaine",
        month_tab: "Mois",
        new_event: "Nouvel événement",
        icon_save: "Sauver",
        icon_cancel: "Annuler",
        icon_details: "Détails",
        icon_edit: "Editer",
        icon_delete: "Masquer",
        confirm_closing: "Confirmez-vous la fermeture ?",
        confirm_deleting: "Confirmez-vous le masquage ?",
        section_description: "Description",
        section_time: "Période",
        full_day: "Toute la journée",
        message_ok: "Oui",
        message_cancel: "Annuler",
    }
}
scheduler.attachEvent("onLightbox", function () {
    var section = scheduler.formSection("description");
    section.control.disabled = true;
});
scheduler.attachEvent("onTemplatesReady", () => {
    scheduler.templates.event_header = (start, end, e) => {
        console.log(e);
        return e;
    }
})

scheduler.attachEvent("onClick", (id, e) => {
    window.open(url + id, "_blank");
    return true;
})

var interval = setInterval(function () {
    if (document.getElementsByTagName("app")[0].innerHTML != undefined && document.getElementsByTagName("app")[0].innerHTML != "Chargement de la page...") {
        clearInterval(interval);
    }
    if (document.getElementById("calendrier") != null) {
        clearInterval(interval);
        scheduler.init("calendrier", Date.now(), "month")
    }
}, 100)

function addDXCalendearEvent(formation, color) {
    scheduler.addEvent({
        start_date: formation.startDate,
        end_date: formation.endDate,
        text: `${formation.name}, à ${formation.location} Lien : ${url}${formation.id}`,
        id: formation.id,
        color: color
    });
}

function removeDXCalendearEvent(formation) {
    scheduler.parse([{
        id: formation.id,
        start_date: formation.startDate,
        end_date: formation.endDate
    }])
    scheduler.deleteEvent(formation.id);
}

function exportDXCalendar() {
    scheduler.exportToICal({
        //server: "https://localhost:5001/calendrier"
    });
}

function display(a) {
    console.log(a);
}