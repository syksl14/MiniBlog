$(function () {
    $("#tabGeneralSettings").load("/SiteSettings/General", {});
    $("#tabMailSettings").load("/SiteSettings/Email", {});
    $("#tabAdvancedSettings").load("/SiteSettings/Advanced", {});
});