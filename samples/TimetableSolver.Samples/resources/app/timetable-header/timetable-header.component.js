/// <reference path="../../angular/angular.d.ts" />
(function(ng){
'use stric';
ng.module('timetable').directive('timetableHeader', function () {
    return {
        controller: TimetableHeaderComponent,
        controllerAs: 'timetableHeaderCtrl',
		link: TimetableHeaderLink,
        template: Template(),
        require: '^timetable',
        restrict: 'A'
    }
});

TimetableHeaderComponent.$inject = ['$scope'];
function TimetableHeaderComponent($scope){
    var ctrl = this;
	ctrl.init = init;
	function init(weekDays){
		ctrl.weekDays = $scope.$eval(weekDays);
		ctrl.weekDays.sort(function(a,b) {return (a.weekDayNumber > b.weekDayNumber) ? 1 : ((b.weekDayNumber > a.weekDayNumber) ? -1 : 0);} );
		ctrl.weekDayTimes = [];
		for(var i = 0; i < ctrl.weekDays.length; i++){
			var weekday = ctrl.weekDays[i];
			for(var j = 1; j < weekday.lessonsPerDay + 1; j++){
				ctrl.weekDayTimes.push(j);
			}
		}	

	}
}

function TimetableHeaderLink(scope, element, attrs){
	scope.timetableHeaderCtrl.init(attrs.timetableHeader);
}

function Template(){
    return `<tr>
	<th rowspan="2">
        Name
	</th>
    <th data-ng-repeat="weekDay in ::timetableHeaderCtrl.weekDays" colspan="{{weekDay.lessonsPerDay}}">{{weekDay.name}}</th>
</tr>
<tr>
	<th data-ng-repeat="weekDayTime in ::timetableHeaderCtrl.weekDayTimes track by $index">
	{{weekDayTime}}
	</th>
</tr>`;
}

})(angular)