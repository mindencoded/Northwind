using System;

namespace S3K.RealTimeOnline.Commons
{
    public interface IContainer
    {
        T Resolve<T>();

        object Resolve(Type type);
    }
}