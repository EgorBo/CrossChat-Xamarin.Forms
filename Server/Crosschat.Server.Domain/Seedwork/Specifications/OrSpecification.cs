using System;
using System.Linq.Expressions;

namespace Crosschat.Server.Domain.Seedwork.Specifications
{
    /// <summary>
    /// A Logic OR Specification
    /// </summary>
    /// <typeparam name="T">Type of entity that check this specification</typeparam>
    public sealed class OrSpecification<T>
         : CompositeSpecification<T>
         where T : class
    {
        #region Members

        private Specification<T> _RightSideSpecification = null;
        private Specification<T> _LeftSideSpecification = null;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Default constructor for AndSpecification
        /// </summary>
        /// <param name="leftSide">Left side specification</param>
        /// <param name="rightSide">Right side specification</param>
        public OrSpecification(Specification<T> leftSide, Specification<T> rightSide)
        {
            if (leftSide == (Specification<T>)null)
                throw new ArgumentNullException("leftSide");

            if (rightSide == (Specification<T>)null)
                throw new ArgumentNullException("rightSide");

            this._LeftSideSpecification = leftSide;
            this._RightSideSpecification = rightSide;
        }

        #endregion

        #region Composite Specification overrides

        /// <summary>
        /// Left side specification
        /// </summary>
        public override Specification<T> LeftSideSpecification
        {
            get { return _LeftSideSpecification; }
        }

        /// <summary>
        /// Righ side specification
        /// </summary>
        public override Specification<T> RightSideSpecification
        {
            get { return _RightSideSpecification; }
        }
        /// <summary>
        /// <see cref="Specification{TEntity}"/>
        /// </summary>
        /// <returns><see cref="Specification{TEntity}"/></returns>
        public override Expression<Func<T, bool>> SatisfiedBy()
        {
            Expression<Func<T, bool>> left = _LeftSideSpecification.SatisfiedBy();
            Expression<Func<T, bool>> right = _RightSideSpecification.SatisfiedBy();

            return (left.Or(right));
            
        }

        #endregion
    }
}
