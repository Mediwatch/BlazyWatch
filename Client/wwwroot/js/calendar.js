scheduler.config.readonly_form = true;
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
        icon_delete: "Supprimer",
        confirm_closing: "Confirmez-vous la fermeture ?",
        confirm_deleting: "Confirmez-vous la suppression ?",
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

var exist = false;
var queue = {};
var interval = setInterval(function () {
    if (document.getElementsByTagName("app")[0].innerHTML != undefined && document.getElementsByTagName("app")[0].innerHTML != "Loading...") {
        clearInterval(interval);
        console.log("FIN !")
    }
    if (document.getElementById("scheduler_here") != null) {
        scheduler.init("scheduler_here", Date.now(), "month")
        exist = true;
        clearInterval(interval);
        console.log("FIN DE FIN !")
        for (var id in queue) {
            addDXCalendearEvent(queue[id][0], queue[id][1])
        }
    }
}, 100)

function addDXCalendearEvent(formation, color) {

    console.log("AJOUT", formation)

    if (exist) {
        scheduler.addEvent({
            start_date: formation.startDate,
            end_date: formation.endDate,
            text: formation.name,
            id: formation.id,
            color: color
        });
    } else {
        queue[formation.id] = [formation, color]
    }
}

function removeDXCalendearEvent(formation) {
    console.log("SUPPRESSION", formation)

    if (exist) {
        scheduler.parse([{
            id: formation.id,
            start_date: formation.startDate,
            end_date: formation.endDate
        }])
        scheduler.deleteEvent(formation.id);
    } else {
        delete queue[formation.id];
    }
}