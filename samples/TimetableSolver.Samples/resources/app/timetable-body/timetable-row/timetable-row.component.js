/// <reference path="../../../angular/angular.d.ts" />
(function(ng){
'use stric';
ng.module('timetable').directive('timetableRow', function () {
    return {
        controller: TimetableRowComponent,
        controllerAs: 'timetableRowCtrl',
		link: TimetableRowLink,
        require: '^timetableBody',
        restrict: 'A',
        template: Tempalte()
    }
});

TimetableRowComponent.$inject = ['$scope', '$element', '$timeout', '$compile'];
function TimetableRowComponent($scope, $element, $timeout, $compile) {
    var ctrl = this;
    var weekDays;
    var weekDaysMap;
    ctrl.init = init;

    function init(timetableMemberAttr, weekDaysAttr) {
        ctrl.timetableMember = $scope.$eval(timetableMemberAttr);
        weekDays = $scope.$eval(weekDaysAttr);
        mapWeekDays(weekDays);
        ctrl.weekDayTimes = Object.keys(weekDaysMap);
    }

    $timeout(afterRender, 0);

    function afterRender(){
        var html = `
        <div class="element" title="$subjectName $teachingGroupName">
        $shortName
        </div>`;

        for(var i = 0; i < ctrl.timetableMember.elements.length; i++){
            var timetableElement = ctrl.timetableMember.elements[i];
            var weekDayTimeIndex = weekDaysMap[timetableElement.dayOfWeek * 100 + timetableElement.dayTime];
            var td = $element.find('td').eq(weekDayTimeIndex + 1).find('div').eq(0);

            var elementHtml = html.replace('$subjectName', timetableElement.subjectName)
            .replace('$teachingGroupName', timetableElement.teachingGroupName)
            .replace('$shortName', timetableElement.shortName);

            td.append($compile(elementHtml)($scope));
        }
    }

    function mapWeekDays(weekDays) {
        weekDaysMap = {};
        var index = 0;
        for (var i = 0; i < weekDays.length; i++) {
            var weekday = weekDays[i];
            for (var j = 1; j < weekday.lessonsPerDay + 1; j++) {
                weekDaysMap[weekday.weekDayNumber*100 + j] = index;
                index++;
            }
        }
    }
}

function TimetableRowLink(scope, element, attrs) {
    scope.timetableRowCtrl.init(attrs.timetableRow, attrs.weekDays);
}

function Tempalte(){
    return `<td>{{timetableRowCtrl.timetableMember.name}}</td>
    <td data-ng-repeat="weekDayTime in ::timetableRowCtrl.weekDayTimes track by $index">
        <div class="table-cell">
        </div>
    </td>`;
}

})(angular)