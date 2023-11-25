$(document).ready(function () {
    $.ajax({
        type: 'Get',
        url: '/Control/Panel/UserCount',
        success: function (res) {            
            $("#uyeler").text(res.length);
        }
    })

    $.ajax({
        type: 'Get',
        url: '/Control/Panel/EducationCount',
        success: function (res) {           
            $("#egitim").text(res);
        }
    })
    
    $.ajax({
        type: 'Get',
        url: '/Control/Panel/TrainingLessons',
        success: function (res) {
            $("#derslerim").text(res);
        }
    })

    $.ajax({
        type: 'Get',
        url: '/Control/Panel/MyTrainings',
        success: function (res) {
            $("#satinAldigim").text(res);
        }
    })

})