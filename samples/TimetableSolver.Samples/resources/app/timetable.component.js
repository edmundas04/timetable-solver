/// <reference path="../angular/angular.d.ts" />
(function(ng){
'use stric';
ng.module('timetable').component('timetable', {
    controller: TimetableComponent,
    controllerAs: 'timetableCtrl',
    template: Template()
});


TimetableComponent.$inject = [];
function TimetableComponent(){
    var ctrl = this;
    ctrl.weekDays = data.weekDays;
    ctrl.timetableMembers = data.timetableMembers;
}


function Template(){
    return `<table>
    <thead timetable-header="timetableCtrl.weekDays"></thead>
    <tbody timetable-body="timetableCtrl.timetableMembers" week-days="timetableCtrl.weekDays"></thead>
</table>`;
}

})(angular)