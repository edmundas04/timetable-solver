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
    ctrl.resetFilters = resetFilters;
    
    function resetFilters() {
        for(var i = 0; i < ctrl.timetableMembers.length; i++){
            ctrl.timetableMembers[i].isVisible = true;;
        }
    }
}


function Template(){
    return `
    <button data-ng-click="timetableCtrl.resetFilters()">Reset filter</button>
    <table>
    <thead timetable-header="timetableCtrl.weekDays"></thead>
    <tbody timetable-body="timetableCtrl.timetableMembers" week-days="timetableCtrl.weekDays"></thead>
</table>`;
}

})(angular)