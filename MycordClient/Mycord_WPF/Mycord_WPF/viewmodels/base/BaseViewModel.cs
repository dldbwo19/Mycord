using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Mycord_WPF
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string property_name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }

    
        protected async Task RunCommand(Expression<Func<bool>> running, Func<Task> action)
        {
            // 프로퍼티 GetVaule()를 실행함으로써 boolean 값을 얻을수 있다.
            if (running.Compile().Invoke())
            {
                return;
            }

            // 람다 식 "() => xxxx.Property"를 "xxxx.Property"로 바꾼다.
            var expression = (running as LambdaExpression).Body as MemberExpression;

            // 프로퍼티 정보를 얻는다.
            PropertyInfo propertyInfo = (PropertyInfo)expression.Member;
            // 타겟 = 해당 인스턴스
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            // 얻어온 프로퍼티를 실행 중으로 바꾼다.
            propertyInfo.SetValue(target, true);

            try
            {
                await action();
            }
            finally
            {
                propertyInfo.SetValue(target, false);
            }
        }
    }
}
