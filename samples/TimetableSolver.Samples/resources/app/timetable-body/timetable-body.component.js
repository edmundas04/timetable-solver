/// <reference path="../../angular/angular.d.ts" />
(function(ng){
'use stric';
ng.module('timetable').directive('timetableBody', function () {
    return {
        controller: TimetableBodyComponent,
        controllerAs: 'timetableBodyCtrl',
		link: TimetableBodyLink,
        template: Template(),
        require: '^timetable',
        restrict: 'A'
    }
});

TimetableBodyComponent.$inject = ['$scope'];
function TimetableBodyComponent($scope){
    var ctrl = this;
    ctrl.init = init;

    function init(timetableMembers, weekDays) {
        ctrl.weekDays = $scope.$eval(weekDays);
		ctrl.weekDays.sort(function(a,b) {return (a.weekDayNumber > b.weekDayNumber) ? 1 : ((b.weekDayNumber > a.weekDayNumber) ? -1 : 0);} );
		ctrl.weekDayTimes = [];
		for(var i = 0; i < ctrl.weekDays.length; i++){
			var weekday = ctrl.weekDays[i];
			for(var j = 1; j < weekday.lessonsPerDay + 1; j++){
				ctrl.weekDayTimes.push(j);
			}
        }

        ctrl.timetableMembers = $scope.$eval(timetableMembers);
    }
}

function TimetableBodyLink(scope, element, attrs){
	scope.timetableBodyCtrl.init(attrs.timetableBody, attrs.weekDays);
}

function Template(){
    return `<tr data-ng-repeat="timetableMember in ::timetableBodyCtrl.timetableMembers" timetable-row="timetableMember" week-days="timetableBodyCtrl.weekDays"></tr>`;
}

})(angular)